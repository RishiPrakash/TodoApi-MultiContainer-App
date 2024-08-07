using Microsoft.Azure.Cosmos;
using TodoApi;

interface IDbClient {
   public Task<ItemResponse<Todo>> AddAsync(Todo todoItem);

   public Task<List<Todo>> GetAllAsync();
   
}