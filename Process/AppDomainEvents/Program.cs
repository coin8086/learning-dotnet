using System.Threading.Tasks;
using System;

namespace AppDomainEvents
{

    class Program
    {
        static async Task Main(string[] args)
        {
            int timeToWait = -1; //in seconds
            bool raiseException = false;

            try
            {
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "-t":
                            timeToWait = int.Parse(args[++i]);
                            break;
                        case "-e":
                            raiseException = true;
                            break;
                        default:
                            throw new ArgumentException($"Unrecognized argument '{args[i]}'.");
                    }
                }
                if (timeToWait < 0)
                {
                    timeToWait = 5;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Argument error: {ex}");
                return;
            }


            Console.WriteLine("Hello, AppDomain!");

#pragma warning disable CS8622
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;
#pragma warning restore
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            if (raiseException)
            {
                throw new ApplicationException("App exception.");
            }
            else
            {
                await Task.Delay(timeToWait * 1000);
            }

            Console.WriteLine("Bye, AppDomain!");
        }

        /*
         * NOTE
         *
         * The event will not be raised if the process is killed from outside.
         */
        private static void ProcessExit(object sender, EventArgs e)
        {
            var type = sender == null ? "" : sender.GetType().ToString();
            Console.Error.WriteLine($"ProcessExit: sender: '{sender}', sender type: '{type}', e: '{e}'");
            Console.Error.Flush();
        }

        /*
         * NOTE
         *
         * The sender is null under .Net (8.0) while it is non-null (that is System.AppDomain) under .Net Framework (4.7.1).
         */
        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var type = sender == null ? "" : sender.GetType().ToString();
            Console.Error.WriteLine($"UnhandledException: sender: '{sender}', sender type: '{type}', e: '{e}'");
            Console.Error.Flush();
        }
    }

}
