using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;

namespace DISample
{
    class Program
    {
        static void BasicTest()
        {
            Console.WriteLine("================= BasicTest Start =================");
            var services = new ServiceCollection();
            services.Add(ServiceDescriptor.Singleton<IServiceA, ServiceA>());
            services.Add(ServiceDescriptor.Transient<IServiceB, ServiceB>());
            services.Add(ServiceDescriptor.Singleton(typeof(IServiceX<>), typeof(ServiceX<>)));
            var provider = ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);
            var sb = provider.GetRequiredService<IServiceB>();
            sb.Say();
            sb = provider.GetRequiredService<IServiceB>();
            sb.Say();
            var sx = provider.GetRequiredService<IServiceX<Program>>();
            sx.Log();
            Console.WriteLine("================= BasicTest End =================");
        }

        interface IBase
        {
            void Speak();
        }

        class Base : IBase
        {
            public void Speak()
            {
                Console.WriteLine($"{ToString()}: {this.GetHashCode()}");
            }
        }

        interface ISub : IBase
        {
            void Say();
        }

        class Sub : Base, ISub
        {
            public void Say()
            {
                throw new NotImplementedException();
            }
        }

        static void SubclassingTest()
        {
            Console.WriteLine("================= SubclassingTest Start =================");
            var services = new ServiceCollection();
            services.Add(ServiceDescriptor.Singleton<ISub, Sub>());
            var provider = ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);

            //This will cause the following error though ISub, who is the descendent of IBase, is registered.
            //Unhandled exception. System.InvalidOperationException: No service for type 'DISample.Program+IBase' has been registered.
            var sb = provider.GetRequiredService<IBase>();

            sb.Speak();
            Console.WriteLine("================= SubclassingTest End =================");
        }

        static void Main(string[] args)
        {
            SubclassingTest();
        }
    }
}
