using Microsoft.Extensions.Logging;
using PluginInterface;
using System.Reflection;

namespace PluginApp;

class Program
{
    static Type[] SharedTypes = [typeof(IPlugin), typeof(ILogger)];

    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            return;
        }
        var pluginAssemblyPath = args[0];

        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddSimpleConsole(options =>
            {
                options.TimestampFormat = "HH:mm:ss.fff ";
            });
            builder.SetMinimumLevel(LogLevel.Debug);
        });

        ILogger logger = loggerFactory.CreateLogger<Program>();
        logger.LogInformation("Begin");
        var plugin = CreatePluginObject(pluginAssemblyPath, logger);
        plugin.DoSomething();
        logger.LogInformation("End");
    }
    static IPlugin CreatePluginObject(string pluginAssemblyPath, ILogger? logger = null)
    {
        try
        {
            var assembly = LoadPluginAssembly(pluginAssemblyPath, logger);
            var type = GetPluginClassType(assembly);
            var instance = (Activator.CreateInstance(type, logger) as IPlugin)!;
            return instance;
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "Error when creating plugin instance.");
            throw;
        }
    }

    static Assembly LoadPluginAssembly(string pluginAssemblyPath, ILogger? logger = null)
    {
        var loadContext = new PluginAssemblyLoadContext(pluginAssemblyPath, SharedTypes, logger);
        return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginAssemblyPath)));
    }

    static Type GetPluginClassType(Assembly assembly)
    {
        foreach (Type type in assembly.GetTypes())
        {
            if (typeof(IPlugin).IsAssignableFrom(type))
            {
                return type;
            }
        }
        throw new ApplicationException($"Can't find a type that implements IPlugin in {assembly} from {assembly.Location}.");
    }
}
