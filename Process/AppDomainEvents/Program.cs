using System.Threading.Tasks;
using System;
using AppDomainEventsDep;

namespace AppDomainEvents
{

    class Program
    {
        static async Task Main(string[] args)
        {
            int timeToWait = 0; //in seconds
            bool raiseException = false;
            string assemblyName = null;

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
                        case "-a":
                            assemblyName = args[++i];
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

            //The two events only work for dynamic assembly loading
            AppDomain.CurrentDomain.AssemblyLoad += AssemblyLoad;
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;

            await Task.Delay(timeToWait * 1000);

            if (assemblyName != null)
            {
                AppDomain.CurrentDomain.Load(assemblyName);
            }

            if (raiseException)
            {
                throw new ApplicationException("App exception.");
            }

            Hello.Say();

            Console.WriteLine("Bye, AppDomain!");
        }

        private static void AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            var type = sender == null ? "" : sender.GetType().ToString();
            var msg = @"
AssemblyLoad:
- sender: '{0}'
- sender type: '{1}'
- assembly: '{2}'
";
            Console.Error.WriteLine(string.Format(msg, sender, type, args.LoadedAssembly));
            Console.Error.Flush();
        }

        private static System.Reflection.Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var type = sender == null ? "" : sender.GetType().ToString();
            var msg = @"
AssemblyResolve:
- sender: '{0}'
- sender type: '{1}'
- requesting assembly: '{2}'
- assembly name: '{3}'
";
            Console.Error.WriteLine(string.Format(msg, sender, type, args.RequestingAssembly, args.Name));
            Console.Error.Flush();
            return null;
        }

        /*
         * NOTE
         *
         * The event will not be raised if the process is killed from outside.
         */
        private static void ProcessExit(object sender, EventArgs args)
        {
            var type = sender == null ? "" : sender.GetType().ToString();
            var msg = @"
ProcessExit:
- sender: '{0}'
- sender type: '{1}'
- args: '{2}'
";
            Console.Error.WriteLine(string.Format(msg, sender, type, args));
            Console.Error.Flush();
        }

        /*
         * NOTE
         *
         * The sender is null under .Net (8.0) while it is non-null (that is System.AppDomain) under .Net Framework (4.7.1).
         */
        private static void UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            var type = sender == null ? "" : sender.GetType().ToString();
            var ex = (Exception)args.ExceptionObject;
            var msg = @"
UnhandledException:
- sender: '{0}'
- sender type: '{1}'
- exception: '{2}'
- terminating: {3}
";
            Console.Error.WriteLine(string.Format(msg, sender, type, ex, args.IsTerminating));
            Console.Error.Flush();
        }
    }

}
