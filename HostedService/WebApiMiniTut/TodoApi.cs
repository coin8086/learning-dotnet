using Microsoft.EntityFrameworkCore;

namespace WebApiMiniTut;

class TodoApi
{
    private ILogger? _logger;

    public TodoApi(ILogger<TodoApi>? logger = null)
    {
        _logger = logger;
        _logger?.LogInformation("TodoApi is created.");
    }

    public async Task<IResult> GetAllTodos(TodoDb db)
    {
        return TypedResults.Ok(await db.Todos.Select(x => new TodoItemDTO(x)).ToArrayAsync());
    }

    public async Task<IResult> GetCompleteTodos(TodoDb db)
    {
        return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).Select(x => new TodoItemDTO(x)).ToListAsync());
    }

    public async Task<IResult> GetTodo(int id, TodoDb db)
    {
        return await db.Todos.FindAsync(id)
            is Todo todo
                ? TypedResults.Ok(new TodoItemDTO(todo))
                : TypedResults.NotFound();
    }

    public async Task<IResult> CreateTodo(TodoItemDTO todoItemDTO, TodoDb db)
    {
        var todoItem = new Todo
        {
            IsComplete = todoItemDTO.IsComplete,
            Name = todoItemDTO.Name
        };

        db.Todos.Add(todoItem);
        await db.SaveChangesAsync();

        todoItemDTO = new TodoItemDTO(todoItem);

        return TypedResults.Created($"/todoitems/{todoItem.Id}", todoItemDTO);
    }

    public async Task<IResult> UpdateTodo(int id, TodoItemDTO todoItemDTO, TodoDb db)
    {
        var todo = await db.Todos.FindAsync(id);

        if (todo is null) return TypedResults.NotFound();

        todo.Name = todoItemDTO.Name;
        todo.IsComplete = todoItemDTO.IsComplete;

        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    public async Task<IResult> DeleteTodo(int id, TodoDb db)
    {
        if (await db.Todos.FindAsync(id) is Todo todo)
        {
            db.Todos.Remove(todo);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }
}

static class TodoApiWebApplicationExtensions
{
    public static WebApplication MapTodoApi(this WebApplication app, string path)
    {
        //NOTE: TodoDb is a scoped service so it cannot be get like this
        //var db = app.Services.GetRequiredService<TodoDb>();

        var logger = app.Services.GetService<ILogger<TodoApi>>();
        var apiInstance = new TodoApi(logger);

        var routeBuilder = app.MapGroup(path);
        routeBuilder.MapGet("/", apiInstance.GetAllTodos);
        routeBuilder.MapGet("/complete", apiInstance.GetCompleteTodos);
        routeBuilder.MapGet("/{id}", apiInstance.GetTodo);
        routeBuilder.MapPost("/", apiInstance.CreateTodo);
        routeBuilder.MapPut("/{id}", apiInstance.UpdateTodo);
        routeBuilder.MapDelete("/{id}", apiInstance.DeleteTodo);

        return app;
    }
}
