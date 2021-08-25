using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void ShowConfig(IConfiguration root)
        {
            Console.WriteLine("Configuration Tree:");
            foreach (var section in root.GetChildren())
            {
                ShowConfigSection(section, 1);
            }
        }

        public static void ShowConfigSection(IConfigurationSection section, int level)
        {
            var indent = new string(' ', level * 2);
            Console.WriteLine($"{indent}Path={section.Path}, Key={section.Key}, Value={section.Value}");
            foreach (var subsec in section.GetChildren())
            {
                ShowConfigSection(subsec, level + 1);
            }
        }
    }
}
