using Microsoft.EntityFrameworkCore;
using Todos.WebApi.Context;
using Todos.WebApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseInMemoryDatabase("MyDb");
});

var app = builder.Build();


app.MapGet("/todos/create", (string work, ApplicationDbContext context) =>
{
    Todo todo = new()
    {
        Work = work,
    };

    context.Add(todo);
    context.SaveChanges();
    return new
    {
        Message = "todo create is successfull"
    };
});

app.MapGet("/todos/getall", (ApplicationDbContext context) =>
{
    var todos = context.Todos.ToList();
    return todos;
});

app.Run();
