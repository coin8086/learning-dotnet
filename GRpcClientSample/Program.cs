using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GRpcClientSample
{
    class Program
    {

        static async Task Main(string[] args)
        {
            // The port number must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("http://localhost:5000");
            Console.WriteLine("SayHello:");
            var client = new Greeter.GreeterClient(channel);
            using var call2 = client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });
            var headers = await call2.ResponseHeadersAsync;
            Console.WriteLine("Headers:");
            foreach (var h in headers)
            {
                Console.WriteLine(h);
            }
            var reply = await call2.ResponseAsync;
            Console.WriteLine("Greeting from server: " + reply.Message);
            Console.WriteLine("Trailers:");
            foreach (var h in call2.GetTrailers())
            {
                Console.WriteLine(h);
            }
            Console.WriteLine();

            var client2 = new Example.ExampleClient(channel);

            Console.WriteLine("Unary:");
            var request = new ExampleRequest() { PageIndex = 1, PageSize = 3, IsDescending = false };
            var response = await client2.UnaryCallAsync(request);
            Console.WriteLine($"Request: PageIndex={request.PageIndex}, PageSize={request.PageSize}, IsDescending={request.IsDescending}");
            Console.WriteLine($"Response: {string.Join(", ", response.Titles)}");
            Console.WriteLine();

            Console.WriteLine("Server Stream:");
            Console.WriteLine($"Request: PageIndex={request.PageIndex}, PageSize={request.PageSize}, IsDescending={request.IsDescending}");
            using (var source = new CancellationTokenSource())
            {
                using (var call = client2.StreamingFromServer(request, cancellationToken: source.Token))
                {
                    try
                    {
                        int i = 0;
                        while (await call.ResponseStream.MoveNext(source.Token))
                        {
                            var response2 = call.ResponseStream.Current;
                            Console.WriteLine($"Response: {string.Join(", ", response2.Titles)}");
                            if (++i > 2)
                            {
                                //This will cancel the client request and thus MoveNext will throw an exception(from server ?).
                                source.Cancel();
                            }
                        }
                    }
                    catch (Grpc.Core.RpcException)
                    {
                        Console.WriteLine("Request was cancled by client.");
                    }
                }
            }
            Console.WriteLine();

            Console.WriteLine("Client Stream:");
            using (var source = new CancellationTokenSource())
            {
                using (var call = client2.StreamingFromClient(cancellationToken: source.Token))
                {
                    try
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            //If request is cancled, then:
                            //1) No response can be gotten from server(call.ResponseAsync)
                            //2) Calls of call.RequestStream.WriteAsync and call.ResponseAsync will throw exception RpcException
                            //if (i == 2)
                            //{
                            //    source.Cancel();
                            //}
                            Console.WriteLine($"Request: PageIndex={request.PageIndex}, PageSize={request.PageSize}, IsDescending={request.IsDescending}");
                            await call.RequestStream.WriteAsync(request);
                            await Task.Delay(TimeSpan.FromSeconds(1));
                        }
                        await call.RequestStream.CompleteAsync();
                        var response3 = await call.ResponseAsync;
                        Console.WriteLine($"Response: {string.Join(", ", response3.Titles)}");
                    }
                    catch (Grpc.Core.RpcException)
                    {
                        Console.WriteLine("Request was cancled by client.");
                    }
                }
            }
            Console.WriteLine();

            Console.WriteLine("Duplex Stream:");
            using (var source = new CancellationTokenSource())
            {
                using (var call = client2.StreamingBothWays(cancellationToken: source.Token))
                {
                    var responser = Task.Run(async () =>
                    {
                        await foreach (var response4 in call.ResponseStream.ReadAllAsync())
                        {
                            Console.WriteLine($"Response for request: PageIndex={response4.Request.PageIndex}, PageSize={response4.Request.PageSize}, IsDescending={response4.Request.IsDescending}");
                            Console.WriteLine($"Response: {string.Join(", ", response4.Titles)}");
                        }
                    });

                    try
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            //If request is cancled, then:
                            //1) No response can be gotten from server(call.ResponseAsync)
                            //2) Calls of call.RequestStream.WriteAsync and call.ResponseAsync will throw exception RpcException
                            //if (i == 2)
                            //{
                            //    source.Cancel();
                            //}
                            await Task.Delay(TimeSpan.FromSeconds(1));
                            Console.WriteLine($"Request: PageIndex={request.PageIndex}, PageSize={request.PageSize}, IsDescending={request.IsDescending}");
                            await call.RequestStream.WriteAsync(request);
                            request.PageIndex++;
                        }
                        await call.RequestStream.CompleteAsync();
                        await responser;
                    }
                    catch (Grpc.Core.RpcException)
                    {
                        Console.WriteLine("Request was cancled by client.");
                    }
                }
            }

        }
    }
}
