using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace UsingOpenTelemetry;

public static class Globals
{
    #region Custom metrics for the application

    public static Meter GreeterMeter { get; } = new Meter("OtPrGrYa.Example", "1.0.0");

    public static Counter<int> GreetingsCount { get; } = GreeterMeter.CreateCounter<int>("greetings.count", description: "Counts the number of greetings");

    #endregion

    #region Custom ActivitySource for the application

    public static ActivitySource GreeterActivitySource { get; } = new ActivitySource("OtPrGrJa.Example");

    #endregion
}
