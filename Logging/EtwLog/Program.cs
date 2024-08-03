//See https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.eventlog?view=net-8.0

using System.Diagnostics;

namespace EtwLog;

class Program
{
    const string EtwSource = "My ETW Source";
    const string EtwLog = "My ETW Log";

    static void Main(string[] args)
    {
        // Create the source, if it does not already exist. Must be run under Administrator role.
        if (!EventLog.SourceExists(EtwSource))
        {
            //An event log source should not be created and immediately used.
            //There is a latency time to enable the source, it should be created
            //prior to executing the application that uses the source.
            //Execute this sample a second time to use the new source.
            EventLog.CreateEventSource(EtwSource, EtwLog);
            Console.WriteLine($"Created event source {EtwSource}");
            Console.WriteLine("Exiting, execute the application a second time to use the source.");
            // The source is created.  Exit the application to allow it to be registered.
            return;
        }

        // Create an EventLog instance and assign its source.
        EventLog myLog = new EventLog();
        myLog.Source = EtwSource;

        // Write an informational entry to the event log.
        myLog.WriteEntry("some log");
        myLog.WriteEntry("some error", EventLogEntryType.Error);

        EventLog.WriteEntry(EtwSource, "some log 2");
        EventLog.WriteEntry(EtwSource, "some error 2", EventLogEntryType.Error);
    }
}
