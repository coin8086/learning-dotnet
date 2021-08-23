using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WorkerSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Main BEGIN");
            CreateHostBuilder(args).Build().Run();
            Console.WriteLine("Main END");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration((configBuilder) =>
                {
                    Console.WriteLine("ConfigureHostConfiguration BEGIN");
                    Console.WriteLine("Config Sources:");
                    foreach (var source in configBuilder.Sources)
                    {
                        Console.WriteLine(source.ToString());
                    }
                    Console.WriteLine("Config Properties:");
                    foreach (var p in configBuilder.Properties)
                    {
                        Console.WriteLine($"{p.Key}={p.Value.ToString()}");
                    }
                    Console.WriteLine("ConfigureHostConfiguration END\n");
                })
                .ConfigureAppConfiguration((builderContext, configBuilder) =>
                {
                    Console.WriteLine("ConfigureAppConfiguration BEGIN");
                    var hostEnv = builderContext.HostingEnvironment;
                    Console.WriteLine($"EnvironmentName = {hostEnv.EnvironmentName}");
                    Console.WriteLine($"ApplicationName = {hostEnv.ApplicationName}");

                    configBuilder.Sources.Clear();
                    configBuilder.AddJsonFile("appsettings.json", true, true);
                    configBuilder.AddJsonFile($"appsettings.{hostEnv.EnvironmentName}.json", true, true);

                    //Try "MYAPP_Logging__LogLevel__Default=Error dotnet.exe run" and
                    //    "myapp_logging__logLevel__default=Error dotnet.exe run"
                    configBuilder.AddEnvironmentVariables("MYAPP_");

                    //Try "dotnet.exe run --Logging:LogLevel:Default Error" and
                    //    "dotnet.exe run --logging:logLevel:default Error"
                    configBuilder.AddCommandLine(args);

                    var root = configBuilder.Build();
                    ShowConfig(root);
                    Console.WriteLine("ConfigureAppConfiguration END\n");
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddHostedService<WorkerOfAppLifetime>();
                });

        public static void ShowConfig(IConfigurationRoot root)
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
