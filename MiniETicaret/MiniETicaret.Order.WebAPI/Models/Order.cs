namespace MiniETicaret.Order.WebAPI.Models;

public sealed class Order
{
    public Order()
    {
        Id = Guid.CreateVersion7();
    }

    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime CreateAt { get; set; }
}
