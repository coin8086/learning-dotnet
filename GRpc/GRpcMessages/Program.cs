//See https://learn.microsoft.com/en-us/aspnet/core/grpc/protobuf?view=aspnetcore-8.0

using GRpcMessages.Services;

namespace GRpcMessages;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddGrpc();

        //See https://learn.microsoft.com/en-us/aspnet/core/grpc/test-tools?view=aspnetcore-8.0#set-up-grpc-reflection
        builder.Services.AddGrpcReflection();

        var app = builder.Build();

        app.MapGrpcReflectionService();

        // Configure the HTTP request pipeline.
        app.MapGrpcService<CheckService>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}
