using Microsoft.Extensions.Configuration;

namespace ConsoleConfiguration;

// NOTE: Don't forget to
// 1) Add packages Microsoft.Extensions.Configuration*
// 2) Copy "appsettings.json" in project file to output directory
// Refer to https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration


class MyProfile
{
    public string? Name { get; set; }
    
    public int? Age { get; set; }

}

class MyOptions
{
    public MyProfile? Profile { get; set; }

    public string? Id { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        //Optional switch mappings for command line
        var switchMappings = new Dictionary<string, string>()
        {
            { "-n", "Profile:Name" },
            { "-a", "Profile:Age" },
            { "-i", "Id" },
        };

        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true)
            .AddEnvironmentVariables("App_")
            .AddCommandLine(args, switchMappings);

        var configuration = builder.Build();

        //Get an option by indexer
        var Name = configuration["Profile:Name"];
        Console.WriteLine($"Hello, {Name}!");

        //Get options by binding
        var options = configuration.Get<MyOptions>();
        Console.WriteLine($"Id={options?.Id}");
        Console.WriteLine($"Profile.Name={options?.Profile?.Name}");
        Console.WriteLine($"Profile.Age ={options?.Profile?.Age}");

        //Show the structured configuration
        Console.WriteLine("Configuration Tree:");
        ShowConfig(configuration);

        Console.WriteLine("Config Sources:");
        foreach (var source in builder.Sources)
        {
            Console.WriteLine(source.ToString());
        }

        Console.WriteLine("Config Properties:");
        foreach (var p in builder.Properties)
        {
            Console.WriteLine($"{p.Key}={p.Value.ToString()}");
        }
    }

    public static void ShowConfig(IConfiguration root)
    {
        foreach (var section in root.GetChildren())
        {
            ShowConfigSection(section, 1);
        }
    }

    public static void ShowConfigSection(IConfigurationSection section, int level)
    {
        var indent = new string(' ', level * 2);
        Console.WriteLine($"{indent}Path={section.Path}, Key={section.Key}, Value={section.Value}");

        foreach (var child in section.GetChildren())
        {
            ShowConfigSection(child, level + 1);
        }
    }
}
