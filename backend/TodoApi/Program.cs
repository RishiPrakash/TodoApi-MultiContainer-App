using TodoApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Cosmos;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// CORS policy configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", builder =>
    {
        builder.WithOrigins("http://localhost:3000", "http://localhost:8080")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
//here we only need 8080 for now, which is for frontend, but localhost:3000
// is added just for example purpose

builder.Services.AddSingleton<IDbClient, TodoListDbClient>();
builder.Services.AddSingleton<IService, TodoService>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Apply CORS middleware
app.UseCors("AllowLocalhost");

app.MapGet("/", () => "Hello World!");
app.MapGet("/get", async (IService service) =>
{ //Here also the lambda handler can get the param populated by DI Container
    var todos = await service.GetAllAsync();
    return todos;
});
app.MapPost("/add", async (IService service, Todo todo) =>
{
    var addedItem = await service.AddItemAysnc(todo);

    return Results.Created($"/get/{addedItem.id}", addedItem);

});

app.Run();


public class Todo
{
    public string id { get; set; }
    public string? Name { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TodoType? type { get; set; }
}

public enum TodoType
{
    Work,
    Health,
    Wealth,
    Family

}


