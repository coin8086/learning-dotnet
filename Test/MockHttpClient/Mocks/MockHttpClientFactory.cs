using Microsoft.Extensions.DependencyInjection;

namespace MockHttpClient.Mocks;

public static class MockHttpClientFactory
{
    public static IHttpClientFactory Create(HttpResponseMessage response)
    {
        var services = new ServiceCollection();
        services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddHttpMessageHandler<MockHttpHandler>();
        });
        services.AddTransient<MockHttpHandler>((_) =>
        {
            return new MockHttpHandler(response);
        });
        var provider = services.BuildServiceProvider();
        return provider.GetRequiredService<IHttpClientFactory>();
    }
}
