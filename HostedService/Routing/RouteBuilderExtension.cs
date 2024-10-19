namespace WebAppRouting;

static class RouteBuilderExtension
{
    public static IEndpointConventionBuilder MapTerminalMiddlewareToEndpoint<T>(this IEndpointRouteBuilder builder, string pattern, params object?[] args) where T : class
    {
        var app = builder.CreateApplicationBuilder()
            .UseMiddleware<T>(args)
            .Build();
        return builder.Map(pattern, app);
    }
}
