using System.Threading.Tasks;
using System;

namespace AppDomainEvents
{

    class Program
    {
        static async Task Main(string[] args)
        {
            int timeToWait = 0; //in seconds
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
                    timeToWait = 0;
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

            await Task.Delay(timeToWait * 1000);

            if (raiseException)
            {
                throw new ApplicationException("App exception.");
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
            var msg = @"
ProcessExit:
- sender: '{0}'
- sender type: '{1}'
- e: '{2}'
";
            Console.Error.WriteLine(string.Format(msg, sender, type, e));
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
            var ex = (Exception)e.ExceptionObject;
            var msg = @"
UnhandledException:
- sender: '{0}'
- sender type: '{1}'
- exception: '{2}'
- terminating: {3}
";
            Console.Error.WriteLine(string.Format(msg, sender, type, ex, e.IsTerminating));
            Console.Error.Flush();
        }
    }

}
