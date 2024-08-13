using System.Net.Http.Headers;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using TodoApi;

public class TodoListDbClient : IDbClient
{
    private readonly Container _todoListContainer;

    private string cosmosDbEndpoint = "https://cosmos-nosql-db.documents.azure.com:443/";
    public TodoListDbClient()
    {   
        var cosmosClientOptions = new CosmosClientOptions {
            
        };
        var cosmosClient = new CosmosClient(accountEndpoint: cosmosDbEndpoint, tokenCredential: new DefaultAzureCredential());
        _todoListContainer = cosmosClient.GetDatabase("Product").GetContainer("TodoList");
    }

    public async Task<ItemResponse<Todo>> AddAsync(Todo todoItem)
    {    
        return await _todoListContainer.CreateItemAsync<Todo>(todoItem);
    }

    public async Task<List<Todo>> GetAllAsync()
    {
        var queryable = _todoListContainer.GetItemLinqQueryable<Todo>();
        var feed = queryable.Where(p => p.id != null).ToFeedIterator();

        List<Todo> todos = [];

        while (feed.HasMoreResults)
        {   //pagination response
            var response = await feed.ReadNextAsync();
            foreach (var item in response)
            {
                todos.Add(item);
            }
        }

        return todos;
    }
}


















