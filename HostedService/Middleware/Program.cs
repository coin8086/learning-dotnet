//See https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-8.0

namespace Middleware;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();
        var logger = app.Services.GetRequiredService<ILogger<Program>>();

        app.UseMyMiddleware();

        app.UseWhen(
            (context) => context.Request.Query.ContainsKey("check"),
            (app2) => {
                app2.Use(async (context, next) =>
                {
                    logger.LogInformation("Before next call in UseWhen");
                    await next(context);
                    logger.LogInformation("After next call in UseWhen");
                });
            }
        );

        app.MapWhen(
            (context) => context.Request.Query.ContainsKey("branch"), 
            (app2) => {
                app2.Run(async (context) =>
                {
                    logger.LogInformation("Terminal 3");
                    await context.Response.WriteAsync("App3 is running.");
                });
            }
        );

        app.Map("/branch", app2 =>
        {
            app2.Run(async (context) =>
            {
                logger.LogInformation("Terminal 2");
                await context.Response.WriteAsync("App2 is running.");
            });
        });

        /*
         * NOTE
         * 
         * The IApplicationBuilder.Use method is defined as
         * 
         * IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware);
         * 
         * And the delegate is defined as
         * 
         * public delegate Task RequestDelegate(HttpContext context);
         */
        app.Use(next => async (context) =>
        {
            logger.LogInformation("Before next call in Use");
            await next(context);
            logger.LogInformation("after next call in Use");
        });

        /*
         * NOTE
         * 
         * The extension Use method is defined as
         * 
         * public static IApplicationBuilder Use(this IApplicationBuilder app, Func<HttpContext, RequestDelegate, Task> middleware)
         * 
         * It can be implemented like
         * 
         * public static IApplicationBuilder Use(this IApplicationBuilder app, Func<HttpContext, RequestDelegate, Task> middleware) {
         *   return app.Use(next => (context) => {
         *     return middleware(context, next);
         *   });
         * }
         */
        app.Use(async (context, next) =>
        {
            logger.LogInformation("Before next call in extension Use");
            await next(context);
            logger.LogInformation("after next call in extension Use");
        });

        /*
         * NOTE
         * 
         * The extension method is defined as
         * 
         * public static void Run(this IApplicationBuilder app, RequestDelegate handler)
         * 
         * It can be implemeneted like
         * 
         * public static void Run(this IApplicationBuilder app, RequestDelegate handler) {
         *   return app.Use(next => (context) => {
         *     return handler(context);
         *   });
         * }
         */
        app.Run(async (context) =>
        {
            logger.LogInformation("Terminal");
            await context.Response.WriteAsync("App is running.");
        });

        app.Run();
    }
}
