//See https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-logging
//and https://learn.microsoft.com/en-us/aspnet/core/fundamentals/w3c-logger/

using Microsoft.AspNetCore.HttpLogging;

namespace HttpLogging;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpLogging(options => {
            options.LoggingFields = HttpLoggingFields.All;
            //Header traceparent will be "redacted" if it's not added here.
            options.RequestHeaders.Add("traceparent");
        });

        var app = builder.Build();

        app.UseHttpLogging();

        app.Run(async (context) =>
        {
            context.Response.ContentType = "text/plain";
            /*
             * NOTE
             * The request body must be consumed, otherwise the HttpLogging won't log it.
             * It seems a bug since .NET 7.x. See https://github.com/dotnet/aspnetcore/issues/49063
            */
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
