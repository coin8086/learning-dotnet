using Grpc.Net.Client;
using System;
using System.Threading.Tasks;

namespace GRpcClientSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // The port number must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });
            Console.WriteLine("Greeting from server: " + reply.Message);

            var client2 = new Example.ExampleClient(channel);
            var response = await client2.UnaryCallAsync(new ExampleRequest() { PageIndex = 1, PageSize = 3, IsDescending = false });
            Console.WriteLine($"Request: PageIndex={response.Request.PageIndex}, PageSize={response.Request.PageSize}");
            Console.WriteLine($"Response: {string.Join(", ", response.Titles)}");
        }
    }
}
