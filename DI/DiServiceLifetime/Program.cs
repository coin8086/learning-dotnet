namespace DiServiceLifetime;

using Microsoft.Extensions.DependencyInjection;
using DiShare;

class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddTransient<ITransientOperation, Operation>();
        services.AddScoped<IScopedOperation, Operation>();
        services.AddSingleton<ISingletonOperation, Operation>();

        var provider = services.BuildServiceProvider();

        using (var scope = provider.CreateScope())
        {
            Console.WriteLine("In scope 1");
            CheckService(scope.ServiceProvider, "1.1");
            CheckService(scope.ServiceProvider, "1.2");
        }

        using (var scope = provider.CreateScope())
        {
            Console.WriteLine("In scope 2");
            CheckService(scope.ServiceProvider, "2.1");
            CheckService(scope.ServiceProvider, "2.2");
        }
    }

    static void CheckService(IServiceProvider provider, string title)
    {
        var transientOp = provider.GetRequiredService<ITransientOperation>();
        var scopedOp = provider.GetRequiredService<IScopedOperation>();
        var singletonOp = provider.GetRequiredService<ISingletonOperation>();

        Console.WriteLine(title);
        Console.WriteLine($"Transient operation ID:\t{transientOp.Id}\nScoped operation ID:\t{scopedOp.Id}\nSingleton operation ID:\t{singletonOp.Id}");
    }
}
