using Microsoft.AspNetCore.Mvc;

namespace UsingOpenTelemetry.Controllers;

[ApiController]
[Route("/")]
public class HelloController : ControllerBase
{
    private readonly ILogger<HelloController> _logger;

    private readonly IHttpClientFactory _httpClientFactory;

    public HelloController(ILogger<HelloController> logger, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _httpClientFactory = clientFactory;
    }

    [HttpGet]
    public async Task<string> SendGreeting()
    {
        await Task.Yield();

        // Create a new Activity scoped to the method
        using var activity = Globals.Source.StartActivity("GreeterActivity");

        // Log a message
        _logger.LogInformation("Sending greeting");

        // Increment the custom counter
        Globals.GreetingsCount.Add(1);

        // Add a tag to the Activity
        activity?.SetTag("greeting", "Hello World!");

        return "Hello World!";
    }

    [HttpGet("NestedGreeting")]
    public async Task SendNestedGreeting([FromQuery] int nestlevel)
    {
        // Create a new Activity scoped to the method
        using var activity = Globals.Source.StartActivity("NestedGreeterActivity");

        if (nestlevel <= 5)
        {
            // Log a message
            _logger.LogInformation("Sending greeting, level {nestlevel}", nestlevel);

            // Increment the custom counter
            Globals.GreetingsCount.Add(1);

            // Add a tag to the Activity
            activity?.SetTag("nest-level", nestlevel);

            //HttpContext.Response.Headers.ContentType = "text/plain";
            await HttpContext.Response.WriteAsync($"Nested Greeting, level: {nestlevel}\r\n");

            if (nestlevel > 0)
            {
                var request = HttpContext.Request;
                var url = new Uri($"{request.Scheme}://{request.Host}{request.Path}?nestlevel={nestlevel - 1}");

                // Makes an http call passing the activity information as http headers
                var nestedResult = await _httpClientFactory.CreateClient().GetStringAsync(url);
                await HttpContext.Response.WriteAsync(nestedResult);
            }
        }
        else
        {
            // Log a message
            _logger.LogError("Greeting nest level {nestlevel} too high", nestlevel);
            await HttpContext.Response.WriteAsync("Nest level too high, max is 5");
        }
    }
}
