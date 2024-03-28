using Grpc.Core;
using Grpc.Net.Client;
using GRpcMethods;

namespace GRpcMethodsClient;

//See https://learn.microsoft.com/en-us/aspnet/core/grpc/client?view=aspnetcore-8.0
class Program
{
    static async Task Main(string[] args)
    {
        var channel = GrpcChannel.ForAddress(args[0]);
        var client = new Example.ExampleClient(channel);

        {
            var response = await client.UnaryCallAsync(new ExampleRequest() { Content = "Hello!" });
            Console.WriteLine($"UnaryCallAsync response: {response.Content}");
        }

        using (var call = client.StreamingFromServer(new ExampleRequest() { Content = "Hello!" }))
        {
            Console.WriteLine($"StreamingFromServer response:");
            await foreach (var response in call.ResponseStream.ReadAllAsync())
            {
                Console.WriteLine(response.Content);
            }
        }

        using (var call = client.StreamingFromClient())
        {
            for (var i = 0; i < 3; i++)
            {
                await call.RequestStream.WriteAsync(new ExampleRequest() { Content = i.ToString() });
            }
            await call.RequestStream.CompleteAsync();
            var response = await call;
            Console.WriteLine($"StreamingFromClient response: {response.Content}");
        }

        using (var call = client.StreamingBothWays())
        {
            var readTask = Task.Run(async () =>
            {
                Console.WriteLine("StreamingBothWays response:");
                await foreach (var response in call.ResponseStream.ReadAllAsync())
                {
                    Console.WriteLine(response.Content);
                }
            });

            for (var i = 0; i < 3; i++)
            {
                await call.RequestStream.WriteAsync(new ExampleRequest() { Content = i.ToString() });
            }
            await call.RequestStream.CompleteAsync();
            await readTask;
        }
    }
}
