/*
 * See
 * https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0
 * https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/handle-errors?view=aspnetcore-8.0
 */

namespace ErrorHandling;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var app = builder.Build();

        app.UseMiddleware<ErrorHandler>();

        app.MapGet("/ok", () => "I'm OK!");

        app.MapGet("/error", string () => throw new Exception("Are you OK?"));

        app.Run();
    }
}
