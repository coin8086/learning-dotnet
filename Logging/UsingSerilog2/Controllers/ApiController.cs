using Microsoft.AspNetCore.Mvc;

namespace UsingSerilog2.Controllers;

[ApiController]
[Route("/api")]
public class ApiController : ControllerBase
{
    private readonly ILogger<ApiController> _logger;

    private readonly IServiceUsingLoggerFactory _service;

    public ApiController(ILogger<ApiController> logger, IServiceUsingLoggerFactory service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet("")]
    public async Task<string> Get()
    {
        _logger.LogInformation("A log message.");
        _service.CallLogging();

        await Task.Yield();
        return "Hello!";
    }
}
