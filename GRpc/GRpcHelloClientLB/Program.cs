using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using GRpcHello;
using Microsoft.Extensions.DependencyInjection;

namespace GRpcHelloClientLB;

//See https://learn.microsoft.com/en-us/aspnet/core/grpc/loadbalancing?view=aspnetcore-8.0
class Program
{
    static void Main(string[] args)
    {
        var factory = new StaticResolverFactory(addr => new[]
        {
            new BalancerAddress("localhost", 5020),
            new BalancerAddress("localhost", 5019),
        });

        var services = new ServiceCollection();
        services.AddSingleton<ResolverFactory>(factory);

        var channel = GrpcChannel.ForAddress(
            "static:///my-example-host",
            new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Insecure,
                ServiceProvider = services.BuildServiceProvider(),
                //ServiceConfig = new ServiceConfig() { LoadBalancingConfigs = { new RoundRobinConfig() } }
            });

        for (var i = 0; i < 10; i++)
        {
            var client = new Greeter.GreeterClient(channel);
            var result = client.SayHello(new HelloRequest() { Name = "Rob" });
            Console.WriteLine(result.Message);
        }
    }
}
