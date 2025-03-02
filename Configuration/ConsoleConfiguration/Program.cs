using Microsoft.Extensions.Configuration;
using System.Text.Json;

using static ConfigurationCommon.Utils;

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

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
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
        ShowOptions<MyOptions>(configuration);

        ShowConfigurationTree(configuration);

        ShowConfigurationSources(builder);

        ShowConfigurationProperties(builder);
    }
}
