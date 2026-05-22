using Microsoft.EntityFrameworkCore;
using Todos.Categories.WebAPI.Context;
using Todos.Categories.WebAPI.Dtos;
using Todos.Categories.WebAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

var app = builder.Build();

app.MapGet("/categories/getall", async (ApplicationDbContext context, CancellationToken cancellation) =>
{
    var categories = await context.Categories.ToListAsync(cancellation);

    return categories;
});

app.MapPost("/categories/create", async (CreateCategoryDto request, ApplicationDbContext context, CancellationToken cancellation) =>
{
    bool isNameExists = await context.Categories.AnyAsync(x => x.Name == request.name, cancellation);
    if (isNameExists)
    {
        return Results.BadRequest(
        new
        {
            Message = "Aranılan Kategori zaten mevcut"
        });
    }

    Category category = new()
    {
        Name = request.name
    };

    await context.Categories.AddAsync(category, cancellation);

    await context.SaveChangesAsync(cancellation);
    return Results.Ok(new
    {
        Message = "Category Create is successfull"
    });

});

app.Run();
