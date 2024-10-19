//See https://learn.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-8.0

namespace WebAppRouting;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.UseEndpointChecker("Before UseRouting");

        app.UseRouting();

        app.UseEndpointChecker("After UseRouting");

        app.MapGet("/", (HttpContext context) =>
        {
            return "Hello World!";
        }).WithDisplayName("root");

        app.MapTerminalMiddlewareToEndpoint<ATerminalMiddleware>("/term").WithDisplayName("term");

        app.UseEndpoints(_ => { });

        app.UseEndpointChecker("After UseEndpoints");

        //The terminal middleware will run only when no matching endpoint.
        app.Run((context) =>
        {
            return context.Response.WriteAsync($"No matching endpoint for \"{context.Request.Path}\".");
        });

        app.Run();
    }
}
