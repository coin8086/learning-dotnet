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

        app.UseHttpsRedirection();

        app.MapGet("/data", async () =>
        {
            var cache = app.Services.GetRequiredService<HybridCache>();
            return await cache.GetOrCreateAsync("data", (token) =>
            {
                return ValueTask.FromResult(DateTimeOffset.UtcNow.ToString());
            });
        });

        app.MapDelete("/data", async () =>
        {
            var cache = app.Services.GetRequiredService<HybridCache>();
            await cache.RemoveAsync("data");
        });

        app.Run();
    }
}
