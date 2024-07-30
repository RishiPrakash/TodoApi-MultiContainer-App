using TodoApi;
using Microsoft.EntityFrameworkCore;


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

//Adding database context to DI container
//builder also contains DI container of Services
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


var app = builder.Build();

// Apply CORS middleware
app.UseCors("AllowLocalhost");

app.MapGet("/", () => "Hello World!");
app.MapGet("/get",async (TodoDb db) => { //Here also the lambda handler can get the param populated by DI Container
    var todos = await db.Todos.ToListAsync();
    return todos;
});
app.MapPost("/add", async (Todo todo, TodoDb db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();
    //Here Results is a factory for IResult which is
    // an interface that represents the result of an HTTP operation
    return Results.Created($"/get/{todo.Id}", todo);
   
});

app.Run();

