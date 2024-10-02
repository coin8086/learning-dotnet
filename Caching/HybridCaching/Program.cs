//See https://learn.microsoft.com/en-us/aspnet/core/performance/caching/hybrid?view=aspnetcore-9.0

using Microsoft.Extensions.Caching.Hybrid;

namespace HybridCaching;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddHybridCache();

        var app = builder.Build();

        app.MapGet("/data", async (HybridCache cache) =>
        {
            return await cache.GetOrCreateAsync("data", (token) =>
            {
                return ValueTask.FromResult(DateTimeOffset.UtcNow.ToString());
            });
        });

        app.MapDelete("/data", async (HybridCache cache) =>
        {
            await cache.RemoveAsync("data");
        });

        app.Run();
    }
}
