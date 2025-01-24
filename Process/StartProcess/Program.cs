using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace StartProcess;


class Program
{
    static void ShowUsage()
    {
        const string usage = """
Usage:
StartProcess [options] command [arguments]

where options are:
-c: Capture the stdout and stderr of child process, and save them to files "stdout" and "stderr" separately. This implies option `-w'.
-h: Show this usage help
-s: Start child process in shell. For Windows, it's `cmd /C "command line"'. For Linux, it's `/bin/sh -c "command line"'. When using this option, no `arguments' is allowed, the `command' should wrap the whole command line as a single string to shell.
-w: Wait for child process
""";
        Console.WriteLine(usage);
    }

    class Options : IValidatableObject
    {
        public bool CaptureOutput { get; set; }

        public bool RunInShell { get; set; }

        public bool WaitForExit { get; set; }

        [Required(ErrorMessage = "Command must be specified!")]
        public string? Command { get; set; }

        public IList<string>? Arguments { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RunInShell && Arguments != null)
            {
                yield return new ValidationResult(
                    "A single parameter for a whole command line must be specified when running in Shell!",
                    [nameof(RunInShell), nameof(Arguments)]);
            }
        }
    }

    class ApplicationExit : ApplicationException { }

    static Options ParseCommandLine(string[] args)
    {
        var options = new Options();
        for (var i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-h":
                    ShowUsage();
                    throw new ApplicationExit();
                case "-c":
                    options.CaptureOutput = true;
                    options.WaitForExit = true;
                    break;
                case "-s":
                    options.RunInShell = true;
                    break;
                case "-w":
                    options.WaitForExit = true;
                    break;
                default:
                    options.Command = args[i];
                    var firstArg = i + 1;
                    if (firstArg < args.Length)
                    {
                        var seg = new ArraySegment<string>(args, firstArg, args.Length - firstArg);
                        options.Arguments = new List<string>(seg);
                    }
                    break;
            }
            if (null != options.Command)
            {
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

    static int Main(string[] args)
    {
        Options options;
        try
        {
            options = ParseCommandLine(args);
        }
        catch (ArgumentException)
        {
            return 1;
        }
        catch (ApplicationExit)
        {
            return 0;
        }

        var startInfo = new ProcessStartInfo()
        {
            UseShellExecute = false,
        };

        if (options.CaptureOutput)
        {
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
        }

        if (options.RunInShell)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                startInfo.FileName = "cmd";
                startInfo.ArgumentList.Add("/C");
                startInfo.ArgumentList.Add(options.Command!);
            }
            else
            {
                startInfo.FileName = "/bin/sh";
                startInfo.ArgumentList.Add("-c");
                startInfo.ArgumentList.Add("--");
                startInfo.ArgumentList.Add(options.Command!);
            }
        }
        else
        {
            startInfo.FileName = options.Command!;
            if (options.Arguments != null)
            {
                foreach (var arg in options.Arguments)
                {
                    startInfo.ArgumentList.Add(arg);
                }
            }
        }

        using var process = new Process()
        {
            StartInfo = startInfo,

            //NOTE: This is to avoid the parent process being zombie in some situation. See 
            //https://github.com/dotnet/runtime/issues/21661
            EnableRaisingEvents = true,
        };

        process.Exited += (sender, args) =>
        {
            Console.WriteLine($"Process {process.Id} exited.");
        };

        var stdout = new StringBuilder();
        var stderr = new StringBuilder();
        if (options.CaptureOutput)
        {
            /*
            * NOTE
            *
            * The args.Data doesn't include the EOL if any. So you cannot tell if there's an EOL for
            * a line of output. Here an EOL is always appended by "AppendLine" to our stdout/stderr 
            * variable. That means if the original output doesn't end with an EOL, our stdout/stderr
            * still ends with it. That is by design. See more at
            *
            * https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.datareceivedeventargs?view=net-8.0
            */
            process.OutputDataReceived += (sender, args) => {
                if (args.Data != null)
                {
                    stdout.AppendLine(args.Data);
                }
            };
            process.ErrorDataReceived += (sender, args) => {
                if (args.Data != null)
                {
                    stderr.AppendLine(args.Data);
                }
            };
        }

        Console.Write($"Process is starting for command `{startInfo.FileName}'");
        if (startInfo.ArgumentList.Any())
        {
            Console.WriteLine($" with arguments:");
            foreach (var item in startInfo.ArgumentList)
            {
                Console.WriteLine(item);
            }
        }
        else
        {
            Console.WriteLine(".");
        }

        process.Start();
        Console.WriteLine($"Process ID: {process.Id}");

        if (options.CaptureOutput)
        {
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
        }

        if (options.WaitForExit || options.CaptureOutput)
        {
            process.WaitForExit();
            Console.WriteLine($"Process {process.Id} exited with code: {process.ExitCode}.");
        }

        if (options.CaptureOutput)
        {
            File.WriteAllText("stdout.txt", stdout.ToString());
            File.WriteAllText("stderr.txt", stderr.ToString());
        }

        Console.WriteLine("Exit.");
        return 0;
    }
}
