using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace UsingOpenTelemetry;

public static class Globals
{
    #region metrics

    public static Meter Meter { get; } = new Meter("Example");

    public static Counter<int> GreetingsCount { get; } = Meter.CreateCounter<int>("greetings.count", description: "Counts the number of greetings");

    #endregion

    #region trace

    public static ActivitySource Source { get; } = new ActivitySource("Example");

    #endregion
}
