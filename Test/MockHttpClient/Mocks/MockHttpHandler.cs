namespace MockHttpClient.Mocks;

public class MockHttpHandler : DelegatingHandler
{
    private HttpResponseMessage _response;

    public MockHttpHandler(HttpResponseMessage response)
    {
        _response = response;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_response);
    }
}
