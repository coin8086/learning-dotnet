using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace StartProcess
{
    class Program
    {
        static void ShowUsage()
        {
            const string usage = @"
Usage:
StartProcess [options] command [arguments]

where options are:
-c: Capture the stdout and stderr of child process, and save them to files ""stdout"" and ""stderr"" separately. This implies option `-w'.
-h: Show this usage help
-s: Start child process in shell. For Windows, it's `cmd /C ""command line"". For Linux, it's `/bin/sh -c ""command line""'. When using this option, no `arguments' is allowed, the `command' should wrap the whole command line as a single string to shell.
-w: Wait for child process
";
            Console.WriteLine(usage);
        }

        static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                ShowUsage();
                return 1;
            }

            bool captureOuput = false;
            bool useShell = false;
            bool waitSubprocess = false;
            string command = null;
            IList<string> argList = null;
            for (var i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-h":
                        ShowUsage();
                        return 0;
                    case "-c":
                        captureOuput = true;
                        waitSubprocess = true;
                        break;
                    case "-s":
                        useShell = true;
                        break;
                    case "-w":
                        waitSubprocess = true;
                        break;
                    default:
                        command = args[i];
                        var firstArg = i + 1;
                        if (firstArg < args.Length)
                        {
                            var seg = new ArraySegment<string>(args, firstArg, args.Length - firstArg);
                            argList = new List<string>(seg);
                        }
                        break;
                }
                if (null != command)
                {
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(command))
            {
                Console.WriteLine("Error: no command!\n");
                ShowUsage();
                return 1;
            }
            if (useShell && argList != null)
            {
                Console.WriteLine("Shell accepts only a single command line parameter!\n");
                return 1;
            }

            var startInfo = new ProcessStartInfo()
            {
                UseShellExecute = false,
            };

            if (captureOuput)
            {
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
            }

            if (useShell)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    startInfo.FileName = "cmd";
                    startInfo.ArgumentList.Add("/C");
                    startInfo.ArgumentList.Add(command);
                }
                else
                {
                    startInfo.FileName = "/bin/sh";
                    startInfo.ArgumentList.Add("-c");
                    startInfo.ArgumentList.Add(command);
                }
            }
            else
            {
                startInfo.FileName = command;
                if (argList != null)
                {
                    foreach (var arg in argList)
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

            var stdout = new StringBuilder();
            var stderr = new StringBuilder();
            if (captureOuput)
            {
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

            Console.WriteLine($"Starting child process `{startInfo.FileName}' with arguments:");
            foreach (var item in startInfo.ArgumentList)
            {
                Console.WriteLine(item);
            }

            process.Start();

            if (captureOuput)
            {
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            }

            if (waitSubprocess || captureOuput)
            {
                process.WaitForExit();
                Console.WriteLine($"Child process exited with code: {process.ExitCode}.");
            }

            if (captureOuput)
            {
                File.WriteAllText("stdout.txt", stdout.ToString());
                File.WriteAllText("stderr.txt", stderr.ToString());
            }

            return 0;
        }
    }
}
