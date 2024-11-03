using System.Net.Http.Json;

namespace MockHttpClient.SUT;

public class UserServiceOnHttpClientFactory
{
    private IHttpClientFactory _httpClientFactory;

    public UserServiceOnHttpClientFactory(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<User> GetUserAsync(int id)
    {
        var httpClient = _httpClientFactory.CreateClient();
        //Assume a URI for a user API
        var uri = $"https://server/api/users/{id}";
        var response = await httpClient.GetAsync(uri);

        response.EnsureSuccessStatusCode();

        var user = await response.Content.ReadFromJsonAsync<User>();
        if (user == null)
        {
            throw new InvalidDataException();
        }
        return user;
    }
}
