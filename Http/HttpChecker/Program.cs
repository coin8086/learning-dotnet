#define USE_HTTP_LOGGING

//See https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-logging for more about HTTP logging

using Microsoft.AspNetCore.HttpLogging;

namespace HttpChecker;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

#if USE_HTTP_LOGGING
        builder.Services.AddHttpLogging(options => {
            options.LoggingFields = HttpLoggingFields.All;
            options.CombineLogs = true;
        });
#else
        builder.AddHttpCheckerOptions();
#endif

        var app = builder.Build();

#if USE_HTTP_LOGGING
        app.UseHttpLogging();
#else
        app.UseHttpChecker();

#endif

        app.UseRouting();

        app.MapGet("/", (HttpContext httpContext) =>
        {
            return "Welcome to home!";
        });

        app.UseEndpoints(_ => { });

        //The terminal middleware will run only when no matching endpoint.
        app.Run(async (context) =>
        {
            context.Response.ContentType = "text/plain";
            using var reader = new StreamReader(context.Request.Body);
            var request = await reader.ReadToEndAsync();
            var response = $"""
Your request: {request}
What are you looking for?
""";
            await context.Response.WriteAsync(response);
        });

        app.Run();
    }
}
