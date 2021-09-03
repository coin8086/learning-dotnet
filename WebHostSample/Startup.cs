using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebHostSample
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        //NOTE: ILogger is unavailabe from DI until Configure is called
        //See https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-5.0#create-logs-in-startup
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Use custom middleware class
            app.UseMyMiddleware("Hellooooo!");

            app.Map("/branch", app =>
            {
                app.Run(async context =>
                {
                    await context.Response.WriteAsync("Branch is hit.");
                });
            });

            //The original Use
            app.Use(next => async (context) =>
            {
                logger.LogInformation("Middleware In");
                await next(context);
                logger.LogInformation("Middleware Out");
            });

            app.UseRouting();

            //The extension Use
            app.Use(async (context, next) =>
            {
                logger.LogInformation("Middleware2 In");
                var endpoint = context.GetEndpoint();
                if (endpoint is null)
                {
                    logger.LogInformation("No endpoint is found!");
                }
                else
                {
                    if (endpoint is RouteEndpoint route)
                    {
                        logger.LogInformation($"Endpoint has pattern: {route.RoutePattern.RawText}");
                    }
                    logger.LogInformation($"Endpoint has metadata: {string.Join(", ", endpoint.Metadata)}");
                }

                await next();
                logger.LogInformation("Middleware2 Out");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                }).WithMetadata("All is well!", 100);

                endpoints.MapMyEndpoint("/my");
            });

            app.Run(async context =>
            {
                logger.LogInformation("Run In");
                await context.Response.WriteAsync("Hey! I caught you!");
                logger.LogInformation("Run Out");
            });
        }
    }
}
