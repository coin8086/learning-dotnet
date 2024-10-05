namespace DiInWebApp;

using System.Diagnostics;
using DiShare;

public class DiChecker
{
    private RequestDelegate _next;
    private ILogger _logger;

    public DiChecker(RequestDelegate next, ILogger<DiChecker> logger)
    {
        _next = next;
        _logger = logger;
    }

    /*
     * NOTE
     * 
     * The framework creates a service scope per request (the HttpContext.RequestServices exposes the scoped service provider).
     */
    public Task InvokeAsync(HttpContext context, ITransientOperation transientOp, IScopedOperation scopedOp, ISingletonOperation singletonOp)
    {
        _logger.LogInformation("\nTransient operation ID: {tid}\nScoped operation ID: {scid}\nSingleton operation ID: {sgid}\n",
    transientOp.Id, scopedOp.Id, singletonOp.Id);

        var singletonOp2 = context.RequestServices.GetRequiredService<ISingletonOperation>();
        Trace.Assert(singletonOp2 == singletonOp);
        var scopedOp2 = context.RequestServices.GetRequiredService<IScopedOperation>();
        Trace.Assert(scopedOp2 == scopedOp);
        var transientOp2 = context.RequestServices.GetRequiredService<ITransientOperation>();
        Trace.Assert(transientOp2 != transientOp);

        return _next(context);
    }
}
