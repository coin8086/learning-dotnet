namespace MockHttpClient;

public class TestHttpHandler : DelegatingHandler
{
    private HttpResponseMessage _response;

    public TestHttpHandler(HttpResponseMessage response)
    {
        _response = response;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_response);
    }
}
