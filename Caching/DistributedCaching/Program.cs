//See https://learn.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-8.0&preserve-view=true

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace DistributedCaching;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddDistributedMemoryCache();
        }
        else
        {
            //Add some distributed cache for production
            throw new NotImplementedException();
        }

        var app = builder.Build();

        app.MapGet("/data", async (IDistributedCache cache) =>
        {
            var data = await cache.GetStringAsync("data");
            return data ?? string.Empty;
        });

        app.MapPost("/data", async (IDistributedCache cache, [FromBody]string data) =>
        {
            await cache.SetStringAsync("data", data);
        });

        app.Run();
    }
}
