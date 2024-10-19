using Microsoft.AspNetCore.Diagnostics;
using System.Net.Mime;

namespace ErrorHandling;

public class ExceptionHandler
{
    ILogger _logger;

    public ExceptionHandler(RequestDelegate _, ILogger<ExceptionHandler> logger)
    {
        _logger = logger;
    }

    public Task InvokeAsync(HttpContext context)
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;
        var result = new { Exception = exception?.GetType().FullName, Message = exception?.ToString() };

        _logger.LogError(exception, "Caught exception!");

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = MediaTypeNames.Application.Json;
        return context.Response.WriteAsJsonAsync(result);
    }
}
