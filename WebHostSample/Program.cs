using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebHostSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((builderContext, configBuilder) =>
                {
                    Console.WriteLine("ConfigureAppConfiguration BEGIN");
                    var hostEnv = builderContext.HostingEnvironment;
                    Console.WriteLine($"EnvironmentName = {hostEnv.EnvironmentName}");
                    Console.WriteLine($"ApplicationName = {hostEnv.ApplicationName}");

                    configBuilder.Sources.Clear();
                    configBuilder.AddJsonFile("appsettings.json", true, true);
                    configBuilder.AddJsonFile($"appsettings.{hostEnv.EnvironmentName}.json", true, true);

                    //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-5.0#environment-variables
                    //"The default configuration loads environment variables and command line arguments prefixed with
                    //DOTNET_ and ASPNETCORE_. The DOTNET_ and ASPNETCORE_ prefixes are used by ASP.NET Core for host
                    //and app configuration, but not for user configuration. "
                    configBuilder.AddEnvironmentVariables("MYAPP_");

                    configBuilder.AddCommandLine(args);

                    var root = configBuilder.Build();
                    ShowConfig(root);
                    Console.WriteLine("ConfigureAppConfiguration END\n");
                })
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
