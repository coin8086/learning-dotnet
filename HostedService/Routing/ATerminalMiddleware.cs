namespace WebAppRouting;

class ATerminalMiddleware
{
    public ATerminalMiddleware(RequestDelegate next)
    {
    }

    public Task InvokeAsync(HttpContext context)
    {
        return context.Response.WriteAsync("This is the terminal.");
    }
}
