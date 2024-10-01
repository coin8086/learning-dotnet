//See https://learn.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-8.0&preserve-view=true

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

        app.UseHttpsRedirection();

        app.MapGet("/data", async () =>
        {
            var cache = app.Services.GetRequiredService<IDistributedCache>();
            var data = await cache.GetStringAsync("data");
            return data ?? string.Empty;
        });

        app.MapPost("/data", async (HttpContext context) =>
        {
            var cache = app.Services.GetRequiredService<IDistributedCache>();
            var reader = new StreamReader(context.Request.Body);
            var data = await reader.ReadToEndAsync();
            cache.SetString("data", data);
        });

        app.Run();
    }
}
