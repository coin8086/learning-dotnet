using System.Text;

namespace Routing;

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
            var msg = $"[{_name}]: No endpoint.";
            _logger.LogInformation(msg);
        }
        else
        {
            var strBuilder = new StringBuilder();
            strBuilder.AppendLine($"[{_name}]: Endpoint: \"{endpoint.DisplayName ?? "(unnamed)"}\"");
            if (endpoint is RouteEndpoint route)
            {
                strBuilder.AppendLine($"Endpoint pattern: {route.RoutePattern.RawText}");
            }
            //strBuilder.AppendLine($"Endpoint metadata: \n{string.Join("\n", endpoint.Metadata)}");
            strBuilder.AppendLine($"Endpoint metadata:");
            foreach (var data in endpoint.Metadata)
            {
                strBuilder.AppendLine(data.ToString());
            }
            _logger.LogInformation(strBuilder.ToString());
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