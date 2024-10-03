//See https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-8.0

namespace WebAppMiddleware;

class MyMiddleware
{
    RequestDelegate _next;
    ILogger _logger;

    public MyMiddleware(RequestDelegate next, ILogger<MyMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context /* optional params of (scoped) services by DI */)
    {
        _logger.LogInformation("Before next call in MyMiddleware");
        await _next(context);
        _logger.LogInformation("After next call in MyMiddleware");
    }
}

static class MyMiddlewareExtensions
{
    public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<MyMiddleware>(/* optional params for the constructor of MyMiddleware */);
    }
}
