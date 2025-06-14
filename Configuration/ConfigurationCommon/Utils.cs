using Microsoft.Extensions.Configuration;

namespace ConfigurationCommon;

public static class Utils
{
    public static void ShowOptions<T>(IConfiguration config, string? configPath = null) where T : class
    {
        T? opts;
        if (configPath is null)
        {
            opts = config.Get<T>();
        }
        else
        {
            opts = config.GetSection(configPath).Get<T>();
        }

        var msg = $"""
=======================
{typeof(T).Name}
{opts}

""";
        Console.WriteLine(msg);
    }

    public static void ShowConfigurationTree(IConfiguration root)
    {
        var msg = """
=======================
Config Tree
""";
        Console.WriteLine(msg);
        foreach (var section in root.GetChildren())
        {
            ShowConfigSection(section, 1);
        }
        Console.WriteLine();
    }

    private static void ShowConfigSection(IConfigurationSection section, int level)
    {
        var indent = new string(' ', level * 2);
        var valueType = section.Value?.GetType().ToString() ?? "(null)";
        Console.WriteLine($"{indent}Path='{section.Path}', Key='{section.Key}', Value='{section.Value}' ValueType='{valueType}'");

        foreach (var child in section.GetChildren())
        {
            ShowConfigSection(child, level + 1);
        }
    }

    public static void ShowConfigurationSources(IConfigurationBuilder configBuilder)
    {
        var msg = """
=======================
Config Sources
""";
        Console.WriteLine(msg);
        foreach (var source in configBuilder.Sources)
        {
            Console.WriteLine(source.ToString());
        }
        Console.WriteLine();
    }

    public static void ShowConfigurationProperties(IConfigurationBuilder configBuilder)
    {
        var msg = """
=======================
Config Properties
""";
        Console.WriteLine(msg);
        foreach (var p in configBuilder.Properties)
        {
            Console.WriteLine($"{p.Key}={p.Value.ToString()}");
        }
        Console.WriteLine();
    }
}
