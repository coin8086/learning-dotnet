using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace DISample
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.Add(ServiceDescriptor.Singleton<IServiceA, ServiceA>());
            //services.Add(ServiceDescriptor.Transient<IServiceA, ServiceA>());
            services.Add(ServiceDescriptor.Transient<IServiceB, ServiceB>());
            var provider = ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);
            var sb = provider.GetRequiredService<IServiceB>();
            sb.Say();
            sb = provider.GetRequiredService<IServiceB>();
            sb.Say();
        }
    }
}
