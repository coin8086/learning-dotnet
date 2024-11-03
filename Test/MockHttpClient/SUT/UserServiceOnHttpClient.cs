using System.Net.Http.Json;

namespace MockHttpClient.SUT;

public class UserServiceOnHttpClient
{
    private HttpClient _httpClient;

    public UserServiceOnHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<User> GetUserAsync(int id)
    {
        //Assume a URI for a user API
        var uri = $"https://server/api/users/{id}";
        var response = await _httpClient.GetAsync(uri);

        response.EnsureSuccessStatusCode();

        var user = await response.Content.ReadFromJsonAsync<User>();
        if (user == null)
        {
            throw new InvalidDataException();
        }
        return user;
    }
}
