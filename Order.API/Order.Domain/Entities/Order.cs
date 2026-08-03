namespace Order.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public string CustomerId { get; private set; } = string.Empty;
    public decimal TotalAmount { get; private set; }
    public string Status { get; private set; } = "Pending"; 
    public DateTime CreatedAt { get; private set; }
    
    // Collection of order items
    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public Order(string customerId, decimal totalAmount)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        TotalAmount = totalAmount;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddItem(string productName, decimal price, int quantity)
    {
        var item = new OrderItem(productName, price, quantity);
        _items.Add(item);
    }

    public void UpdateStatus(string status)
    {
        Status = status;
    }

    public static Order Create(string customerId)
    {
        return new Order(customerId, 0); // O inicializando el total en 0 y calculándolo al agregar items
    }
}