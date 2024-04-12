
//See https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient-guidelines
//and https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient

namespace HttpClientSample;

static class HttpResponseMessageExtensions
{
    internal static async Task WriteToConsole(this HttpResponseMessage response)
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

        var body = await response.Content.ReadAsStringAsync();
        if (body != null)
        {
            var maxLength = 1024;
            if (body.Length > maxLength)
            {
                body = body.Substring(0, maxLength) + "...";
            }
            Console.WriteLine(body);
        }
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
                await response.WriteToConsole();
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.ToString());
        }

        try
        {
            //Send
            var request = new HttpRequestMessage(HttpMethod.Head, url);
            using (var response = await client.SendAsync(request))
            {
                await response.WriteToConsole();
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
