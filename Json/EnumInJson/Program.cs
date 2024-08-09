using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EnumInJson;

class Program
{
    public class Weather
    {
        public enum SummaryType
        {
            ColdDay,
            CoolDay, 
            WarmDay,
            HotDay
        }

        public int Temperature { get; set; }

        public SummaryType? Summary { get; set; }
    }

    public class Weather2
    {
        //See https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/converters-how-to?pivots=dotnet-8-0#register-a-custom-converter
        //Put a converter attr here or at a property for the type.
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        public enum SummaryType
        {
            ColdDay,
            CoolDay,
            WarmDay,
            HotDay
        }

        public int Temperature { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SummaryType? Summary { get; set; }
    }

    static void Main(string[] args)
    {
        var weather = new Weather
        {
            Temperature = 25,
            Summary = Weather.SummaryType.WarmDay
        };

        //Enum as int
        var json = JsonSerializer.Serialize(weather);
        Console.WriteLine(json);

        var obj = JsonSerializer.Deserialize<Weather>(json);
        Trace.Assert(obj?.Summary == weather.Summary);

        //Enum as string
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
        };
        json = JsonSerializer.Serialize(weather, options);
        Console.WriteLine(json);

        obj = JsonSerializer.Deserialize<Weather>(json, options);
        Trace.Assert(obj?.Summary == weather.Summary);

        //Enum as string in camel case
        options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
        };
        json = JsonSerializer.Serialize(weather, options);
        Console.WriteLine(json);

        obj = JsonSerializer.Deserialize<Weather>(json, options);
        Trace.Assert(obj?.Summary == weather.Summary);

        //Enum as string by attribute
        var weather2 = new Weather2
        {
            Temperature = 10,
            Summary = Weather2.SummaryType.CoolDay
        };

        var json2 = JsonSerializer.Serialize(weather2);
        Console.WriteLine(json2);

        var obj2 = JsonSerializer.Deserialize<Weather2>(json2);
        Trace.Assert(obj2?.Summary == weather2.Summary);
    }
}
