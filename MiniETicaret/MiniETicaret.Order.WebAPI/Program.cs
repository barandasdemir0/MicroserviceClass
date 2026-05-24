using MiniETicaret.Order.WebAPI.Context;
using MiniETicaret.Order.WebAPI.Dtos;
using MiniETicaret.Order.WebAPI.Models;
using MiniETicaret.Order.WebAPI.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;


BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));


builder.Services.AddSingleton<MongoDbContext>();




var app = builder.Build();

app.MapGet("/", () => "Hello World!");


app.MapGet("/getall", async (MongoDbContext context,IConfiguration configuration) =>
{
    var items = context.GetCollection<Order>("Orders");

    var orders = await items.Find(items => true).ToListAsync();

    List<OrderDto> orderDtos = new();
    Result<List<ProductDto>>? productDtos = new();

    HttpClient httpClient = new();
    string productsEndpoint = $"http://{configuration.GetSection("HttpRequest:Products").Value}/getall";
    var message = await httpClient.GetAsync(productsEndpoint);
    if (message.IsSuccessStatusCode)
    {
        productDtos = await message.Content.ReadFromJsonAsync<Result<List<ProductDto>>>();
    }
    foreach (var order in orders)
    {
        OrderDto orderDto = new()
        {
            Id = order.Id,
            CreateAt = DateTime.Now,
            ProductId = order.ProductId,
            Quantity = order.Quantity,
            Price = order.Price,
            ProductName = productDtos!.Data!.First(p => p.Id == order.ProductId).Name,
        };

        orderDtos.Add(orderDto);
    }

    return Results.Ok(new Result<List<OrderDto>>(orderDtos));
});

app.MapPost("/create",async(MongoDbContext context, List<CreateOrderDto> request) =>
{
    var items = context.GetCollection<Order>("Orders");
    List<Order> orders = new();

    foreach (var item in request)
    {
        Order order = new()
        {
            ProductId = item.ProductId,
            Quantity = item.Quantity,
            Price = item.Price,
            CreateAt = DateTime.Now

        };
        orders.Add(order);
    }

    await items.InsertManyAsync(orders);
    return Results.Ok(new Result<string>("Sipariş Başarıyla oluşturuldu"));
});


app.Run();
