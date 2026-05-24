namespace MiniETicaret.Order.WebAPI.Dtos;

public sealed record CreateOrderDto
(
    Guid ProductId,
    int Quantity,
    decimal Price
);

