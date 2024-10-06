//See https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-8.0&tabs=visual-studio

using Microsoft.EntityFrameworkCore;

namespace WebApiMiniTut;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddSingleton<TodoApi>();

        var app = builder.Build();
        app.MapTodoApi("/todoitems");
        app.Run();
    }
}
