using System.Text.Json;

namespace DynamicConfiguration;

public class MyOptions
{
    public string? Name { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, this.GetType());
    }
}

public class AOptions : MyOptions
{
    public int Number {  get; set; }
}

public class BOptions : MyOptions
{
    public string? Alias { get; set; }
}

public static class MyOptionsIConfigurationExtension
{
    public static MyOptions GetMyOptions(this IConfiguration configuration, string configPath)
    {
        var path = $"{configPath}:Name";
        var name = configuration[path] ?? throw new ArgumentNullException(path);
        Type type;
        switch (name)
        {
            case nameof(AOptions):
                type = typeof(AOptions);
                break;
            case nameof(BOptions):
                type = typeof(BOptions);
                break;
            default:
                throw new ArgumentException($"Invalid option name '{name}'.");
        }
        return (MyOptions)configuration.GetSection(configPath).Get(type)!;
    }
}
