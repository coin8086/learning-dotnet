using Microsoft.Extensions.DependencyInjection;
using MockHttpClient.SUT;
using System.Net;
using System.Net.Http.Json;

namespace MockHttpClient;

public class UserServiceOnHttpClientFactoryTest
{
    [Fact]
    public async Task TestGetUserAsyncOk()
    {
        var user = new User()
        {
            Id = 1,
            Name = "Test",
        };
        var services = new ServiceCollection();
        services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddHttpMessageHandler<TestHttpHandler>();
        });
        services.AddTransient<TestHttpHandler>((_) =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = JsonContent.Create(user) };
            return new TestHttpHandler(response);
        });
        var provider = services.BuildServiceProvider();
        var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();

        var service = new UserServiceOnHttpClientFactory(httpClientFactory);
        var result = await service.GetUserAsync(1);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test", result.Name);
    }

    [Fact]
    public async Task TestGetUserAsyncError()
    {
        var services = new ServiceCollection();
        services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddHttpMessageHandler<TestHttpHandler>();
        });
        services.AddTransient<TestHttpHandler>((_) =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            return new TestHttpHandler(response);
        });
        var provider = services.BuildServiceProvider();
        var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();

        var service = new UserServiceOnHttpClientFactory(httpClientFactory);
        await Assert.ThrowsAsync<HttpRequestException>(async () =>
        {
            await service.GetUserAsync(1);
        });
    }
}
