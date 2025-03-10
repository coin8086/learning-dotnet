using System.Net.Mime;

namespace ErrorHandling;

public class ErrorHandler
{
    private RequestDelegate _next;
    private ILogger _logger;

    public ErrorHandler(RequestDelegate next, ILogger<ErrorHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Caught exception!");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = MediaTypeNames.Text.Plain;
            await context.Response.WriteAsync(ex.ToString());
        }
    }
}
