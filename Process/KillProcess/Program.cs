using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace KillProcess;

static class Program
{
    class Options : IValidatableObject
    {
        public int Pid { get; set; }

        public bool Kill { get; set; }

        public bool KillTree { get; set; }

        public bool Close { get; set; }

        public bool WaitForExit { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Pid == 0)
            {
                yield return new ValidationResult("Process ID must be specified and cannot be zero!", [nameof(Pid)]);
            }
        }
    }

    class ApplicationExit : ApplicationException { }

    static void ShowUsage()
    {
        var usage = """
Usage:
KillProcess [options] pid

where options are:
-k: Kill the process
-t: Kill the process tree
-c: Call Close() on the process object
-w: Wait the process
""";
        Console.WriteLine(usage);
    }

    static Options ParseCommandLine(string[] args)
    {
        var options = new Options();

        for (var i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-h":
                    throw new ApplicationExit();
                case "-k":
                    options.Kill = true;
                    break;
                case "-t":
                    options.KillTree = true;
                    break;
                case "-c":
                    options.Close = true;
                    break;
                case "-w":
                    options.WaitForExit = true;
                    break;
                default:
                    if (!int.TryParse(args[i], out var pid))
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        options.Pid = pid;
                    }
                    break;
            }
        }

        var results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(options, new ValidationContext(options), results, false))
        {
            Console.WriteLine(results.First().ErrorMessage);
            ShowUsage();
            throw new ArgumentException(results.First().ErrorMessage);
        }
        return options;
    }

    static void Main(string[] args)
    {
        Options options;
        try
        {
            options = ParseCommandLine(args);
        }
        catch
        {
            return;
        }

        using var process = Process.GetProcessById(options.Pid);

        process.Exited += (sender, args) =>
        {
            Console.WriteLine($"Process {options.Pid} exited.");
        };

        if (options.KillTree)
        {
            Console.WriteLine($"Kill process tree of {options.Pid}");
            process.Kill(true);
        }
        else if (options.Kill)
        {
            Console.WriteLine($"Kill process {options.Pid}");
            process.Kill();
        }

        if (options.Close)
        {
            Console.WriteLine($"Close process {options.Pid}");
            process.Close();
        }

        if (options.WaitForExit)
        {
            Console.WriteLine($"Wait for process {options.Pid}");
            process.WaitForExit();
        }

        Console.WriteLine("Exit.");
    }
}
