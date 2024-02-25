using System.Diagnostics;

namespace StartProcessUsingShell;

class Program
{
    static int Main(string[] args)
    {
        if (args.Length != 1)
        {
            ShowUsage();
            return 1;
        }

        //This doesn't work since FileName must be set to a non-empty value!
        var startInfo = new ProcessStartInfo() { UseShellExecute = true, Arguments = args[0], FileName = "" }; 

        using var process = new Process() { StartInfo = startInfo };
        process.Start();
        process.WaitForExit();
        return process.ExitCode;
    }

    private static void ShowUsage()
    {
        var usage = @"
StartProcessUsingShell ""{command line quoted in a single string}""
";
        Console.WriteLine(usage);
    }
}
