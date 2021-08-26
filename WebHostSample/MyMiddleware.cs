using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebHostSample
{
    public class MyMiddleware
    {
        private readonly RequestDelegate _next;

        private ILogger<MyMiddleware> _logger;

        private string _msg;

        //NOTE: logger is passed by DI, and the order of logger and msg doesn't matter, though UseMyMiddleware
        public MyMiddleware(RequestDelegate next, ILogger<MyMiddleware> logger, string msg)
        {
            _next = next;
            _logger = logger;
            _msg = msg;
            _logger.LogInformation($"MyMiddleware::MyMiddleware: {msg}");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation($"MyMiddleware::InvokeAsync {_msg} In");
            await _next(context);
            _logger.LogInformation($"MyMiddleware::InvokeAsync {_msg} Out");
        }
    }

    public static class MyMiddlewareExtension
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder, string msg)
        {
            return builder.UseMiddleware<MyMiddleware>(msg);
        }
    }
}
