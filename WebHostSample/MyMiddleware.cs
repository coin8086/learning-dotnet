using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebHostSample
{
    public class MyMiddleware
    {
        private readonly RequestDelegate _next;

        private string _msg;

        public MyMiddleware(RequestDelegate next, string msg)
        {
            Console.WriteLine($"MyMiddleware::MyMiddleware: {msg}");
            _next = next;
            _msg = msg;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine($"MyMiddleware::InvokeAsync {_msg} In");
            await _next(context);
            Console.WriteLine($"MyMiddleware::InvokeAsync {_msg} Out");
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
