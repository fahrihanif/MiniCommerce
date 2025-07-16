namespace MiniCommerce.API.Entities;

public class User
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    
    // Navigation properties
    public Account Account { get; set; }
    public Cart Cart { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}