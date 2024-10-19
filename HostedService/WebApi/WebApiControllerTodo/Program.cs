
//See https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-8.0&tabs=visual-studio

using Microsoft.EntityFrameworkCore;
using WebApiControllerTodo.Models;

namespace WebApiControllerTodo;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));

        var app = builder.Build();
        app.MapControllers();
        app.Run();
    }
}
