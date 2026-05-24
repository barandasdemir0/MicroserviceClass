namespace MiniETicaret.ShoppingCarts.WebAPI.Dtos;

public sealed record CreateShoppingCartDto
    (
    Guid ProductId,
    int Quantity
    );
