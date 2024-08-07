using TodoApi;

class TodoService : IService
{
    private readonly IDbClient _dbClient;
    public TodoService(IDbClient dbClient)
    {
        _dbClient = dbClient;
    }

    public async Task<Todo> AddItemAysnc(Todo item)
    {
        try
        {   
            item.id = Guid.NewGuid().ToString();
            
            var itemResponse = await _dbClient.AddAsync(item);
            if (itemResponse.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return itemResponse.Resource;
            }
            throw new Exception("Failed to add the item");
        }
        catch (System.Exception)
        {
            Console.WriteLine("Failed to add the item");
            throw;
        }
    }

    public async Task<List<Todo>> GetAllAsync(){
        try
        {
            return await _dbClient.GetAllAsync();    
        }
        catch (System.Exception)
        {
            
            throw;
        }
        
    }
}