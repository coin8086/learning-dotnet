using System;
using System.Collections.Generic;
using System.Diagnostics;

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
-h: Show this usage help
-w: Wait for subprocess
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
                if (command != null)
                {
                    break;
                }
            }

            var startInfo = new ProcessStartInfo(command)
            {
                UseShellExecute = false,
            };
            if (argList != null)
            {
                foreach (var arg in argList)
                {
                    startInfo.ArgumentList.Add(arg);
                }
            }
            using var process = new Process()
            {
                StartInfo = startInfo,

                //NOTE: This is to avoid the parent process being zombie in some situation. See 
                //https://github.com/dotnet/runtime/issues/21661
                EnableRaisingEvents = true,
            };
            process.Start();
            if (waitSubprocess)
            {
                process.WaitForExit();
            }
            return 0;
        }
    }
}
