using Microsoft.EntityFrameworkCore;
using MiniETicaret.ShoppingCarts.WebAPI.Context;
using MiniETicaret.ShoppingCarts.WebAPI.Dtos;
using MiniETicaret.ShoppingCarts.WebAPI.Models;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"));
});



var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/getall", async (ApplicationDbContext context, IConfiguration configuration,CancellationToken cancellationToken) =>
{
    List<ShoppingCart> shoppingCarts = await context.ShoppingCarts.ToListAsync(cancellationToken);

    HttpClient httpClient = new();

    string productsEndpoint = $"http://{configuration.GetSection("HttpRequest:Products").Value}/getall";
    var message = await httpClient.GetAsync(productsEndpoint);

    Result<List<ProductDto>>? products = new();
    if (message.IsSuccessStatusCode)
    {
        products = await message.Content.ReadFromJsonAsync<Result<List<ProductDto>>>();
    }


    List<ShoppingCartDto> response = shoppingCarts.Select(s => new ShoppingCartDto()
    {
        Id = s.Id,
        ProductId = s.ProductId,
        Quantity = s.Quantity,
        ProductName = products!.Data!.First(p => p.Id == s.ProductId).Name,
        ProductPrice = products.Data!.First(p => p.Id == s.ProductId).Price
    }).ToList();



    return new Result<List<ShoppingCartDto>>(response);


});

app.MapPost("/create", async (CreateShoppingCartDto request, ApplicationDbContext context, CancellationToken cancellationToken) =>
{
    ShoppingCart shoppingCart = new()
    {
        ProductId = request.ProductId,
        Quantity = request.Quantity
    };

    await context.AddAsync(shoppingCart, cancellationToken);
    await context.SaveChangesAsync(cancellationToken);

    return Results.Ok(new Result<string>("Ürün Başarıyla Sepete Eklendi"));
});


app.MapGet("/createOrder", async (ApplicationDbContext context, IConfiguration configuration, CancellationToken cancellationToken) =>
{
    List<ShoppingCart> shoppingCarts = await context.ShoppingCarts.ToListAsync(cancellationToken);

    HttpClient httpClient = new();

    string productsEndpoint = $"http://{configuration.GetSection("HttpRequest:Products").Value}/getall";
    var productsMessage = await httpClient.GetAsync(productsEndpoint);

    Result<List<ProductDto>>? products = new();
    if (productsMessage.IsSuccessStatusCode)
    {
        products = await productsMessage.Content.ReadFromJsonAsync<Result<List<ProductDto>>>();
    }


    List<CreateOrderDto> response = shoppingCarts.Select(s => new CreateOrderDto()
    {
        ProductId = s.ProductId,
        Quantity = s.Quantity,
        Price = products!.Data!.First(p => p.Id == s.ProductId).Price
    }).ToList();

    string ordersEnpoint = $"http://{configuration.GetSection("HttpRequest:Orders").Value}/create";

    string json = JsonSerializer.Serialize(response);
    var content = new StringContent(json, Encoding.UTF8, "application/json");



    var orderMessage = await httpClient.PostAsync(ordersEnpoint,content);
    if (orderMessage.IsSuccessStatusCode)
    {
        List<ChangeProductStockDto> changeProductStockDtos = shoppingCarts.Select(s => new ChangeProductStockDto
        (
            s.ProductId,
            s.Quantity
        )).ToList();

        string Productjson = JsonSerializer.Serialize(changeProductStockDtos);
        var Productcontent = new StringContent(Productjson, Encoding.UTF8, "application/json");

        string productsChangeEndpoint = $"http://{configuration.GetSection("HttpRequest:Products").Value}/changeProductStock";


        await httpClient.PostAsync(productsChangeEndpoint, Productcontent);

        context.RemoveRange(shoppingCarts);
        await context.SaveChangesAsync(cancellationToken);
    }

    return Results.Ok(new Result<string>("Sipariş Başarıyla oluşturuldu"));
});




using (var scoped = app.Services.CreateScope())
{
    var srv = scoped.ServiceProvider;
    var context = srv.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}


app.Run();
