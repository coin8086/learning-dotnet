using Microsoft.EntityFrameworkCore;

namespace WebApiMiniTodo;

class TodoApi
{
    private ILogger? _logger;

    public TodoApi(ILogger<TodoApi>? logger = null)
    {
        _logger = logger;
        _logger?.LogInformation("TodoApi is created.");
    }

    /*
     * NOTE
     *
     * TodoDb is a scoped service while TodoApi is singlton. So TodoDb cannot be injected in a constructor,
     * but in a method. An alternative is to register TodoApi as a scoped service, then TodoDb can be
     * injected in a constructor, like a controller class. But TodoApi as a scoped service requires more
     * code in implementing MapTodoApi. Considering the cost, it may be better to use a controller instead,
     * for TodoApi as a scoped service.
     */
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
    public static IEndpointRouteBuilder MapTodoApi(this IEndpointRouteBuilder builder, string path)
    {
        var apiInstance = builder.ServiceProvider.GetRequiredService<TodoApi>();
        var group = builder.MapGroup(path);
        group.MapGet("/", apiInstance.GetAllTodos);
        group.MapGet("/complete", apiInstance.GetCompleteTodos);
        group.MapGet("/{id}", apiInstance.GetTodo);
        group.MapPost("/", apiInstance.CreateTodo);
        group.MapPut("/{id}", apiInstance.UpdateTodo);
        group.MapDelete("/{id}", apiInstance.DeleteTodo);
        return builder;
    }
}
