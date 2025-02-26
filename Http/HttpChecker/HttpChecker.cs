using Microsoft.Extensions.Options;
using System.Text;

namespace HttpChecker;

class HttpCheckerOptions
{
    public bool Enabled { get; set; } = false;
}

class HttpChecker
{
    RequestDelegate _next;
    ILogger _logger;
    IOptionsMonitor<HttpCheckerOptions> _options;

    public HttpChecker(RequestDelegate next, ILogger<HttpChecker> logger, IOptionsMonitor<HttpCheckerOptions> options)
    {
        _next = next;
        _logger = logger;
        _options = options;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (_options.CurrentValue.Enabled)
        {
            await CheckRequestAsync(context.Request);
            await _next(context);
            await CheckResponseAsync(context.Response);
        }
        else
        {
            await _next(context);
        }
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
    public static WebApplicationBuilder AddHttpCheckerOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<HttpCheckerOptions>(builder.Configuration.GetSection("HttpChecker"));
        return builder;
    }

    public static IApplicationBuilder UseHttpChecker(this IApplicationBuilder app)
    {
        return app.UseMiddleware<HttpChecker>();
    }
}
