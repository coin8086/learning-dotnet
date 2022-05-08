using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace WriteSharedFile
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                var path = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "WriteSharedFile");
                var info = new ProcessStartInfo(path, "work");

                Console.WriteLine($"Starting processes of `{path}'...");

                for (var i = 0; i < 10; i++)
                {
                    Process.Start(info);
                }
            }
            else
            {
                const string filename = "log.txt";
                const string lockname = "Global\\WriteShared";

                var pid = Process.GetCurrentProcess().Id;
                Console.WriteLine($"Process {pid} is running...");

                using var fileStream = new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                using var fileWriter = new StreamWriter(fileStream);
                using var mutex = new Mutex(false, lockname);
                var random = new Random();
                for (var i = 0; i < 10; i++)
                {
                    mutex.WaitOne();
                    try
                    {
                        fileStream.Seek(0, SeekOrigin.End);
                        fileWriter.WriteLine($"[{pid}][{DateTimeOffset.UtcNow.ToString("o")}] Message {i}");
                        fileWriter.Flush();
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                    Thread.Sleep((int)(random.NextDouble() * 1000));
                }
            }
        }
    }
}
