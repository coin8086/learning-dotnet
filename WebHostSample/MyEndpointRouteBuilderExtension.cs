using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebHostSample
{
    public static class MyEndpointRouteBuilderExtension
    {
        //This shows how a middleware can be used as an endpoint, optionlly with an additional middleware stack.
        public static IEndpointConventionBuilder MapMyEndpoint(this IEndpointRouteBuilder endpoints, string pattern)
        {
            var app = endpoints.CreateApplicationBuilder()
                .Use(next => async (context) =>
                {
                    await context.Response.WriteAsync("My endpoint is reached.");
                })
                .Build();
            return endpoints.Map(pattern, app);
        }
    }
}
