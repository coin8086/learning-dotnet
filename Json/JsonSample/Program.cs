using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonSample
{
    public class WeatherForecast
    {
        public enum SummaryType
        {
            Cold, Cool, Warm, Hot
        }

        public string City { get; set; }
        public DateTimeOffset Date { get; set; }
        public int TemperatureCelsius { get; set; }

        public SummaryType? Summary { get; set; }
    }

    public class Program
    {
        public static void Main()
        {
            var weatherForecast = new WeatherForecast
            {
                City = "Shanghai, China",
                //Date = DateTimeOffset.Parse("2019-08-01"),
                Date = DateTimeOffset.UtcNow,
                TemperatureCelsius = 25,
                Summary = WeatherForecast.SummaryType.Warm
            };
            var array = new WeatherForecast[] { weatherForecast };

            var options = new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
                WriteIndented = true,
            };
            string jsonString = JsonSerializer.Serialize(array, options);
            Console.WriteLine(jsonString);

            var array2 = JsonSerializer.Deserialize<WeatherForecast[]>(jsonString, options);
            Trace.Assert(array2 != null && array2.Length == 1);
            var weatherForecast2 = array2[0];
            Trace.Assert(weatherForecast2.City == weatherForecast.City);
            Trace.Assert(weatherForecast2.Date == weatherForecast.Date);
            Trace.Assert(weatherForecast2.TemperatureCelsius == weatherForecast.TemperatureCelsius);
            Trace.Assert(weatherForecast2.Summary == weatherForecast.Summary);
        }
    }
}
