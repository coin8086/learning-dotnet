//See https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-8.0#outgoing-request-middleware

namespace HttpClientFactory;

public class HttpClientTracker : DelegatingHandler
{
    ILogger _loger;

    public HttpClientTracker(ILogger<HttpClientTracker> logger)
    {
        _loger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        //NOTE: The logging here is only for demo purpose, since
        //IHttpClientFactory logs HTTP requests and responses of its Http clients. 
        _loger.LogInformation($"Start sending to {request.RequestUri}");

        var response = await base.SendAsync(request, cancellationToken);

        _loger.LogInformation($"Response code: {response.StatusCode}");
        return response;
    }
}
