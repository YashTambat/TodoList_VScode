using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

SQLitePCL.Batteries.Init();

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Todos") ?? "Data Source=Todos.db";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSqlite<TodoDb>(connectionString);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TodoApp API",
        Description = "Managing your todos",
        Version = "v1"
    });
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoApp API V1");
});

// Use CORS
app.UseCors();

app.MapGet("/", () => "Hello World!");
app.MapGet("/todos", async (TodoDb db) => await db.Todos.ToListAsync());

app.MapGet("/todo/{id}", async (TodoDb db, int id) => await db.Todos.FindAsync(id));

app.MapPut("/todo/{id}", async (TodoDb db, Todo updatetodo, int id) =>
{
    var todo = await db.Todos.FindAsync(id);
    if (todo is null) return Results.NotFound();
    todo.Name = updatetodo.Name;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapPost("/todo", async (TodoDb db, TodoApp.Models.Todo todo) =>
{
    await db.Todos.AddAsync(todo);
    await db.SaveChangesAsync();
    return Results.Created($"/todo/{todo.Id}", todo);
});

app.MapDelete("/todo/{id}", async (TodoDb db, int id) =>
{
    var todo = await db.Todos.FindAsync(id);
    if (todo is null)
    {
        return Results.NotFound();
    }
    db.Todos.Remove(todo);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();

