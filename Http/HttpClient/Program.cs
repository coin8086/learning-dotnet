
//See https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient-guidelines
//and https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient

namespace HttpClientSample;

static class HttpResponseMessageExtensions
{
    internal static void WriteToConsole(this HttpResponseMessage response)
    {
        if (response is null)
        {
            return;
        }

        var request = response.RequestMessage;
        Console.Write($"{request?.Method} ");
        Console.Write($"{request?.RequestUri} ");
        Console.Write($"HTTP/{request?.Version} ");
        Console.WriteLine((int)response.StatusCode);
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length != 1)
        {
            throw new ArgumentException();
        }
        var url = args[0];
        var client = new HttpClient() /* { DefaultRequestVersion = HttpVersion.Version20 } */;

        try
        {
            //Get
            using (var response = await client.GetAsync(url))
            {
                response.WriteToConsole();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.ToString());
        }

        try
        {
            //Send
            var request = new HttpRequestMessage(HttpMethod.Options, url);
            using (var response = await client.SendAsync(request))
            {
                response.WriteToConsole();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
