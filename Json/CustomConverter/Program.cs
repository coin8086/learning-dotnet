//See https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/converters-how-to?pivots=dotnet-8-0

using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CustomConverter;

public class DateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTimeOffset.ParseExact(reader.GetString()!, "MM/dd/yyyy", CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
    }
}

public class Matter
{
    [JsonConverter(typeof(DateTimeOffsetJsonConverter))]
    public DateTimeOffset? StartedAt { get; set; }

    public DateTimeOffset? EndedAt { get; set; }

    public override string ToString()
    {
        return $"(\nStartedAt={StartedAt?.ToString("u")}\nEndedAt={EndedAt?.ToString("u")}\n)";
    }
}

class Program
{
    static void Main(string[] args)
    {
        var matter = new Matter()
        { 
            StartedAt = DateTimeOffset.Now - TimeSpan.FromHours(1),
            EndedAt = DateTimeOffset.Now
        };
        Console.WriteLine(matter);
        Console.WriteLine("------------------------");

        var json = JsonSerializer.Serialize(matter, new JsonSerializerOptions() { WriteIndented = true });
        Console.WriteLine(json);
        Console.WriteLine("------------------------");

        var matter2 = JsonSerializer.Deserialize<Matter>(json);
        Trace.Assert(matter2 != null);
        Console.WriteLine(matter2);
    }
}
