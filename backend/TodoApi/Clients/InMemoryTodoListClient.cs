using Microsoft.Azure.Cosmos;

class InMemoryTodoListClient : IDbClient
{
    private readonly Dictionary<string, Todo> _todoItems = new();

    public Task<Todo> AddAsync(Todo todoItem)
    {   

        _todoItems.Add(todoItem.id, todoItem);
        return Task.FromResult(todoItem);
        
    }

    public Task<List<Todo>> GetAllAsync()
    {
        return Task.FromResult(_todoItems.Values.ToList());
    }

    
}