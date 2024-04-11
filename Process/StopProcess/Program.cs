using System.Diagnostics;
using System.Runtime.InteropServices;

namespace StopProcess;

class Program
{
    static void Main(string[] args)
    {
        string filename;
        string arguments;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            filename = "powershell";
            arguments = "-command \"&sleep 120\"";
        }
        else
        {
            filename = "bash";
            arguments = "-c \"sleep 120\"";
        }

        var startInfo = new ProcessStartInfo()
        {
            UseShellExecute = false,
            FileName = filename,
            Arguments = arguments,
        };

        using var process = new Process()
        {
            StartInfo = startInfo,
            EnableRaisingEvents = true,
        };
        process.Start();

        Console.WriteLine($"Process ID: {process.Id}");

        //NOTE: It seems Close/Dispose doesn't end a process!
        //Besides, Kill after Dispose will throw an exception and the process remains!

        //Console.WriteLine("Press any key to close process...");
        //Console.ReadKey(true);
        //process.Close();

        //Console.WriteLine("Press any key to dispose process...");
        //Console.ReadKey(true);
        //process.Dispose();

        //Console.WriteLine("Press any key to kill process...");
        //Console.ReadKey(true);
        //process.Kill();

        //Console.WriteLine("Wait for exit...");
        //process.WaitForExit();
        //Console.WriteLine($"Exit code: {process.ExitCode}");

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey(true);
    }
}
