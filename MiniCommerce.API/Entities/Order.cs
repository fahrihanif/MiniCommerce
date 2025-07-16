namespace MiniCommerce.API.Entities;

public class Order
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? VoucherId { get; set; }
    public string ShippingAddress { get; set; }
    public ShippingMethod ShippingMethod { get; set; }
    public int TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
    
    public User User { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

public enum OrderStatus { Pending, OnProcess, Success, Failure }

public enum ShippingMethod { Instant, SameDay, Regular, Cargo }
