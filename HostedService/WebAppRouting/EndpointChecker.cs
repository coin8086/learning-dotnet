namespace WebAppRouting;

class EndpointChecker
{
    private ILogger _logger;

    private RequestDelegate _next;

    private string _name;

    public EndpointChecker(ILogger<EndpointChecker> logger, RequestDelegate next, string name)
    {
        _logger = logger;
        _next = next;
        _name = name;
    }

    public Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint is null)
        {
            _logger.LogInformation("{name}: No endpoint!", _name);
        }
        else
        {
            _logger.LogInformation("{name}: Endpoint \"{endpoint}\" is found.", _name, endpoint.DisplayName ?? "(unnamed)");
            if (endpoint is RouteEndpoint route)
            {
                _logger.LogInformation("{name}: Endpoint pattern: {pattern}", _name, route.RoutePattern.RawText);
            }
            _logger.LogInformation("{name}: Endpoint metadata: \n{metadata}", _name, string.Join("\n", endpoint.Metadata));
        }
        return _next(context);
    }
}

static class IApplicationBuilderExtensions
{
    public static IApplicationBuilder UseEndpointChecker(this IApplicationBuilder app, string name)
    {
        return app.UseMiddleware<EndpointChecker>(name);
    }
}