//See https://learn.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-8.0

namespace Routing;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.UseEndpointChecker("UseRouting Before");

        app.UseRouting();

        app.UseEndpointChecker("UseRouting After");

        app.MapGet("/", (HttpContext context) =>
        {
            return "Hello World!";
        }).WithDisplayName("root");

        app.MapTerminalMiddlewareToEndpoint<ATerminalMiddleware>("/term").WithDisplayName("term");

        app.UseEndpoints(_ => { });

        app.UseEndpointChecker("UseEndpoints After");

        //The terminal middleware will run only when no matching endpoint.
        app.Run((context) =>
        {
            return context.Response.WriteAsync($"No matching endpoint for \"{context.Request.Path}\".");
        });

        app.Run();
    }
}
