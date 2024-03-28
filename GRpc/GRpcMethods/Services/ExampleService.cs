using Grpc.Core;
using System.Text;

namespace GRpcMethods.Services;

public class ExampleService : GRpcMethods.Example.ExampleBase
{
    public override Task<ExampleResponse> UnaryCall(ExampleRequest request, ServerCallContext context)
    {
        return Task.FromResult(new ExampleResponse() { Content = request.Content });
    }

    public override async Task StreamingFromServer(ExampleRequest request, IServerStreamWriter<ExampleResponse> responseStream, ServerCallContext context)
    {
        for (var i = 0; i < 3 && !context.CancellationToken.IsCancellationRequested; i++)
        {
            await responseStream.WriteAsync(new ExampleResponse() { Content = i.ToString() }, context.CancellationToken);
            await Task.Delay(1000, context.CancellationToken);
        }
    }

    public override async Task<ExampleResponse> StreamingFromClient(IAsyncStreamReader<ExampleRequest> requestStream, ServerCallContext context)
    {
        var strBuilder = new StringBuilder();
        await foreach (var msg in requestStream.ReadAllAsync(context.CancellationToken))
        {
            strBuilder.Append(msg.Content);
        }
        return new ExampleResponse() { Content = strBuilder.ToString() };
    }

    public override async Task StreamingBothWays(IAsyncStreamReader<ExampleRequest> requestStream, IServerStreamWriter<ExampleResponse> responseStream, ServerCallContext context)
    {
        await foreach (var msg in requestStream.ReadAllAsync(context.CancellationToken))
        {
            await responseStream.WriteAsync(new ExampleResponse() { Content = msg.Content }, context.CancellationToken);
            await Task.Delay(1000, context.CancellationToken);
        }
    }
}
