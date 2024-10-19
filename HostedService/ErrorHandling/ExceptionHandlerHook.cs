using Microsoft.AspNetCore.Diagnostics;

namespace ErrorHandling;

/*
 * NOTE
 * 
 * The hook is optionally and will be called only when app.UseExceptionHandler has been called. See more about the IExceptionHandler at
 * https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0&preserve-view=true#iexceptionhandler
 */
public class ExceptionHandlerHook : IExceptionHandler
{
    ILogger _logger;

    public ExceptionHandlerHook(ILogger<ExceptionHandlerHook> logger)
    {
        _logger = logger;
    }

    public ValueTask<bool> TryHandleAsync(HttpContext context, Exception ex, CancellationToken cancellationToken)
    {
        _logger.LogError(ex, "Caught ex!");

        //Exception can be handled here by writing HTTP response and return true.

        return ValueTask.FromResult(false);
    }
}
