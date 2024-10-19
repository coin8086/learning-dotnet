/*
 * See
 * https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0
 * https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/handle-errors?view=aspnetcore-8.0
 */

//#define USE_EXCEPTION_HANDLER

namespace ErrorHandling;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

#if USE_EXCEPTION_HANDLER
        builder.Services.AddExceptionHandler<ExceptionHandlerHook>();
#endif

        var app = builder.Build();

#if !USE_EXCEPTION_HANDLER
        app.UseMiddleware<ErrorHandler>();
#endif

#if USE_EXCEPTION_HANDLER
        app.UseExceptionHandler(app2 =>
        {
            app2.UseMiddleware<ExceptionHandler>();
        });
#endif

        app.MapGet("/ok", () => "I'm OK!");

        app.MapGet("/error", string () => throw new Exception("Are you OK?"));

        app.Run();
    }
}
