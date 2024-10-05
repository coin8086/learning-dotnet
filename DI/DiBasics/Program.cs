using Microsoft.Extensions.DependencyInjection;

namespace DiBasics;

class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddSingleton(typeof(IServiceX<>), typeof(ServiceX<>));
        services.AddSingleton<IServiceA, ServiceA>();
        services.AddTransient<IServiceB, ServiceB>();

        var provider = services.BuildServiceProvider();

        var sb = provider.GetRequiredService<IServiceB>();
        sb.Check();

        sb = provider.GetRequiredService<IServiceB>();
        sb.Check();

        var sx = provider.GetRequiredService<IServiceX<Program>>();
        sx.Check();
    }
}
