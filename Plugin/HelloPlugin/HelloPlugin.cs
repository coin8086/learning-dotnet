using Microsoft.Extensions.Logging;
using PluginInterface;

namespace HelloPlugin;

public class HelloPlugin : IPlugin
{
    private readonly ILogger? _logger;

    public HelloPlugin(ILogger? logger)
    {
        _logger = logger;
    }

    public void DoSomething()
    {
        _logger?.LogInformation("DoSomething");
    }
}
