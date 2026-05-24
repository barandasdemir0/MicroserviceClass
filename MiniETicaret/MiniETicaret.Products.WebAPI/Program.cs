using Bogus;
using Microsoft.EntityFrameworkCore;
using MiniETicaret.Products.WebAPI.Context;
using MiniETicaret.Products.WebAPI.Dtos;
using MiniETicaret.Products.WebAPI.Models;
using TS.Result;

var builder = WebApplication.CreateBuilder(args);

#region

builder.Services.AddDbContext<ApplicatonDbContext>(context =>
{
    context.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});








#endregion

var app = builder.Build();

app.MapGet("/", () => "Hello World!");


app.MapGet("/seedData", (ApplicatonDbContext context) =>
{
    for (int i = 0; i < 100; i++)
    {
        Faker faker = new();
        Product product = new()
        {
            Name = faker.Commerce.ProductName(),
            Price = Convert.ToDecimal(faker.Commerce.Price()),
            Stock = faker.Commerce.Random.Int(1, 100)
        };
        context.Products.Add(product);

    }
    context.SaveChanges();

    return Results.Ok(Result<string>.Succeed("Seed Data başarıyla çalıştırılıp fake veri olulturuldu"));
});

app.MapGet("/getall", async (ApplicatonDbContext context, CancellationToken cancellationToken) =>
{
    var product = await context.Products.OrderBy(x => x.Name).ToListAsync(cancellationToken);
    if (!product.Any())
    {
        return Result<List<Product>>.Failure("Herhangi bir ürün listesi yoktur");
    }

    return Result<List<Product>>.Succeed(product);
});

app.MapPost("/create", async (CreateProductDto requests, ApplicatonDbContext context, CancellationToken cancellationToken) =>
{
    var exisistsProduct = await context.Products.AnyAsync(x => x.Name == requests.Name);
    if (exisistsProduct)
    {
        var response = Result<string>.Failure("Aynı Adda Ürün Mevcuttur");
        return Results.BadRequest(response);
    }

    Product product = new()
    {
        Name = requests.Name,
        Price = requests.Price,
        Stock = requests.Stock
    };

    await context.AddAsync(product, cancellationToken);
    await context.SaveChangesAsync(cancellationToken);

    return Results.Ok(Result<string>.Succeed("Ürün Başarıyla Eklenmiştir"));
});

app.MapPost("/changeProductStock", async (List<ChangeProductStockDto> request,ApplicatonDbContext context, CancellationToken cancellationToken) =>
{
    foreach (var item in request)
    {
        Product? product = await context.Products.FindAsync(item.ProductId,cancellationToken);
        if (product is not null)
        {
            product.Stock -= item.Quantity;
        }
    }
    await context.SaveChangesAsync(cancellationToken);
    return Results.NoContent();
});


using (var scoped = app.Services.CreateScope())
{
    var srv = scoped.ServiceProvider;
    var context = srv.GetRequiredService<ApplicatonDbContext>();
    context.Database.Migrate();
}

app.Run();
