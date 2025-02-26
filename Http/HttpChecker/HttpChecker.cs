using System.Text;

namespace HttpChecker;

class HttpChecker
{
    RequestDelegate _next;
    ILogger _logger;

    public HttpChecker(RequestDelegate next, ILogger<HttpChecker> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await CheckRequestAsync(context.Request);
        await _next(context);
        await CheckResponseAsync(context.Response);
    }

    public static string GetHeadersInString(IHeaderDictionary headers)
    {
        var headerString = new StringBuilder();
        foreach (var (k, v) in headers)
        {
            headerString.AppendLine($"{k}: {v}");
        }
        return headerString.ToString();
    }

    public static async Task<string?> GetBodyInStringAsync(Stream body)
    {
        if (!body.CanRead)
        {
            return null;
        }
        var reader = new StreamReader(body);
        return await reader.ReadToEndAsync();
    }

    private async Task CheckRequestAsync(HttpRequest request)
    {
        var headerString = GetHeadersInString(request.Headers);
        var body = await GetBodyInStringAsync(request.Body);
        var msg = $"""
Incoming HTTP request
{request.Method} {request.Path}

Headers:
{headerString}

Body:
{body ?? "(null)"}
""";
        _logger.LogInformation(msg);
    }

    private async Task CheckResponseAsync(HttpResponse response)
    {
        var headerString = GetHeadersInString(response.Headers);
        var body = await GetBodyInStringAsync(response.Body);
        var msg = $"""
Outgoing HTTP response
{response.StatusCode}

Headers:
{headerString}

Body:
{body ?? "(null)"}
""";
        _logger.LogInformation(msg);
    }
}

static class HttpCheckerExtensions
{
    public static IApplicationBuilder UseHttpChecker(this IApplicationBuilder app)
    {
        return app.UseMiddleware<HttpChecker>();
    }
}
