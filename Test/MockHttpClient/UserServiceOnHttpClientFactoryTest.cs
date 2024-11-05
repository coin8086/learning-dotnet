using MockHttpClient.Mocks;
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
        var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = JsonContent.Create(user) };
        var httpClientFactory = MockHttpClientFactory.Create(response);
        var service = new UserServiceOnHttpClientFactory(httpClientFactory);
        var result = await service.GetUserAsync(1);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test", result.Name);
    }

    [Fact]
    public async Task TestGetUserAsyncError()
    {
        var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        var httpClientFactory = MockHttpClientFactory.Create(response);
        var service = new UserServiceOnHttpClientFactory(httpClientFactory);
        await Assert.ThrowsAsync<HttpRequestException>(async () =>
        {
            await service.GetUserAsync(1);
        });
    }
}
