using Grpc.Net.Client;
using GRpcHello;

namespace GRpcHelloClient;

class Program
{
    static void Main(string[] args)
    {
        var channel = GrpcChannel.ForAddress(args[0]);
        var client = new Greeter.GreeterClient(channel);
        var result = client.SayHello(new HelloRequest() { Name = "Rob" });

        Console.WriteLine(result.Message);
    }
}
