namespace Order.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }

    public OrderItem(string productName, decimal price, int quantity)
    {
        Id = Guid.NewGuid();
        ProductName = productName;
        Price = price;
        Quantity = quantity;
    }
}