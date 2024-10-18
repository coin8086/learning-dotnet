//See https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/parameter-binding?view=aspnetcore-8.0

using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Diagnostics;

namespace WebAppParamBinding;


public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSingleton<SearchService>();

        var app = builder.Build();

        //Bind explicitly
        app.MapGet("/search/{id:int}",
            (
                [FromRoute] int id,
                [FromQuery] string query,
                [FromHeader(Name = "X-Data")] string header,
                [FromBody] Note note,
                [FromServices] SearchService search) =>
            {
                return TypedResults.Ok($"id={id}\nquery={query}\nheader={header}\nnote={note}\nsearch={search}");
            });

        //Bind implicitly
        app.MapGet("/search2/{id:int}",
            (
                int id,
                string query,
                SearchService search) =>
            {
                return TypedResults.Ok($"id={id}\nquery={query}\nsearch={search}");
            });

        //Implicit binding of HTTP body for POST
        app.MapPost("/notes", (Note note) =>
        {
            return TypedResults.Ok($"note.Id={note.Id}\nnote.Summary={note.Summary}\nnote.Time={note.Time}");
        });

        //Optional parameters
        app.MapGet("/search3/{id:int?}",
            (
                int? id,
                string? query) =>
            {
                return TypedResults.Ok($"id={id}\nquery={query}");
            });

        //Special types
        app.MapGet("/search4",
            (HttpContext context, HttpRequest request, HttpResponse response, ClaimsPrincipal principal, CancellationToken cancellationToken) =>
            {
                Trace.Assert(context.Request == request);
                Trace.Assert(context.Response == response);
                Trace.Assert(context.User == principal);
                Trace.Assert(context.RequestAborted == cancellationToken);

                return TypedResults.Ok($"user={principal.Identity?.Name}\ntrace-id={context.TraceIdentifier}");
            });

        /*
         * NOTE:
         * 
         * For more special types, form binding, custom type binding, JSON options for binding, etc., see 
         * 
         * https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/parameter-binding?view=aspnetcore-8.0#binding-precedence
         * 
         * and the head reference.
         */

        app.Run();
    }
}
