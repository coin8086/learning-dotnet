namespace HttpClientFactory;

public class ApiClient : IDisposable
{
    private HttpClient _client;
    private ILogger _logger;

    public ApiClient(HttpClient client, ILogger<ApiClient> logger)
    {
        _client = client;
        _client.BaseAddress = new Uri("http://worldclockapi.com/");
        _logger = logger;
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public async Task<ApiResult?> GetResultAsync()
    {
        _logger.LogInformation("Start getting result");
        var result = await _client.GetFromJsonAsync<ApiResult>("api/json/utc/now");
        return result;
    }
}
