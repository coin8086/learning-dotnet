using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace MutexSample
{
    class Program
    {
        const string GLOBAL_MUTEX_NAME = "Global\\MyMutex";

        const string SUBCOMMAND = "work";

        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                //Parent process
                Mutex mutex;
                if (Mutex.TryOpenExisting(GLOBAL_MUTEX_NAME, out mutex))
                {
                    mutex.Dispose();
                    Console.Error.WriteLine($"Found existing mutext {GLOBAL_MUTEX_NAME}.");
                    return 1;
                }

                //TODO: This doesn't work! Why?
                //var command = System.Reflection.Assembly.GetExecutingAssembly().Location;
                var command = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "MutexSample");
                var startInfo = new ProcessStartInfo(command)
                {
                    UseShellExecute = false,
                };
                startInfo.Arguments = SUBCOMMAND;

                for (var i = 0; i < 4; i++)
                {
                    using var process = new Process()
                    {
                        StartInfo = startInfo,

                        //NOTE: This is to avoid the parent process being zombie in some situation. See 
                        //https://github.com/dotnet/runtime/issues/21661
                        EnableRaisingEvents = true,
                    };
                    Console.Error.WriteLine($"Starting command: {command} {SUBCOMMAND}");
                    process.Start();
                }
            }
            else if (args[0].Equals(SUBCOMMAND, StringComparison.OrdinalIgnoreCase))
            {
                using var mutex = new Mutex(false, GLOBAL_MUTEX_NAME);
                if (mutex.WaitOne(0))
                {
                    Console.Error.WriteLine($"[{Process.GetCurrentProcess().Id}] I accquired the mutex {GLOBAL_MUTEX_NAME}!");
                    Thread.Sleep(60 * 1000);
                    mutex.ReleaseMutex();
                }
                else
                {
                    Console.Error.WriteLine($"[{Process.GetCurrentProcess().Id}] I failed waiting the mutex {GLOBAL_MUTEX_NAME}. Exiting...");
                    return 1;
                }
            }
            else
            {
                Console.Error.WriteLine("Invalid argument!");
                return 1;
            }
            return 0;
        }
    }
}
