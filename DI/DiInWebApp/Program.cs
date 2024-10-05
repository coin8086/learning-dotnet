//See https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-8.0

namespace DiInWebApp;

using DiShare;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddTransient<ITransientOperation, Operation>();
        builder.Services.AddScoped<IScopedOperation, Operation>();
        builder.Services.AddSingleton<ISingletonOperation, Operation>();

        var app = builder.Build();

        app.UseMiddleware<DiChecker>();

        var logger = app.Services.GetRequiredService<ILogger<Program>>();

        app.MapGet("/", (ITransientOperation transientOp, IScopedOperation scopedOp, ISingletonOperation singletonOp) => {
            logger.LogInformation("\nTransient operation ID: {tid}\nScoped operation ID: {scid}\nSingleton operation ID: {sgid}\n", 
                transientOp.Id, scopedOp.Id, singletonOp.Id);
            return "Hello, DI!";
        });

        app.Run();
    }
}
