using Microsoft.AspNetCore.Mvc;

namespace WebWithWorker;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSingleton<Worker>();
        builder.Services.AddHostedService<Worker>((provider) => {
            return provider.GetRequiredService<Worker>();
        });
        var app = builder.Build();

        app.MapGet("/", () => "Hello World!");

        app.MapPost("/stop", async ([FromServices] Worker worker) =>
        {
            await worker.StopAsync(default);
        });
        
        app.MapPost("/start", async ([FromServices] Worker worker) =>
        {
            await worker.StartAsync(default);
        });

        app.Run();
    }
}
