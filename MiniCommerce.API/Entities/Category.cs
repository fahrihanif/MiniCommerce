namespace MiniCommerce.API.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    // Collection navigation property
    public ICollection<Product> Products { get; set; } = new List<Product>();
}