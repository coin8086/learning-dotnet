//See https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient-guidelines
//and https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-8.0

namespace HttpClientFactory;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddHttpClient();
        builder.Services.AddHttpClient("worldclockapi", client =>
        {
            client.BaseAddress = new Uri("http://worldclockapi.com/");
        });

        //The typed client is registered as transient with DI. It can also be configurated like the named client "worldclockapi".
        builder.Services.AddHttpClient<ApiClient>();

        //Multiple message handlers (of base type DelegatingHandler) can be added,
        //which forms the "outgoing request middlewares".
        builder.Services.AddTransient<HttpClientTracker>();
        builder.Services.AddHttpClient("tracker").AddHttpMessageHandler<HttpClientTracker>();

        var app = builder.Build();

        app.MapGet("/basic", async (IHttpClientFactory httpClientFactory, ILogger<Program> logger) =>
        {
            logger.LogInformation($"Get default client. httpClientFactory hashcode: {httpClientFactory.GetHashCode()}");
            using var client = httpClientFactory.CreateClient();
            var result = await client.GetFromJsonAsync<ApiResult>("http://worldclockapi.com/api/json/utc/now");
            return result;
        });

        app.MapGet("/named", async (IHttpClientFactory httpClientFactory, ILogger<Program> logger) =>
        {
            logger.LogInformation($"Get named client. httpClientFactory hashcode: {httpClientFactory.GetHashCode()}");
            using var client = httpClientFactory.CreateClient("worldclockapi");
            var result = await client.GetFromJsonAsync<ApiResult>("api/json/utc/now");
            return result;
        });

        app.MapGet("/typed", async (ApiClient client, ILogger<Program> logger) =>
        {
            logger.LogInformation("Get typed client");
            var result = await client.GetResultAsync();
            return result;
        });

        app.MapGet("/tracked", async (IHttpClientFactory httpClientFactory, ILogger<Program> logger) =>
        {
            logger.LogInformation($"Get tracked client. httpClientFactory hashcode: {httpClientFactory.GetHashCode()}");
            using var client = httpClientFactory.CreateClient("tracker");
            var result = await client.GetFromJsonAsync<ApiResult>("http://worldclockapi.com/xyz");
            return result;
        });

        app.Run();
    }
}
