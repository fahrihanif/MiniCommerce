namespace MiniCommerce.API.Entities;

public class Account
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
    public bool IsActive { get; set; }
    public bool IsEnabled { get; set; }
    public string? RefreshToken { get; set; }
    
    // Navigation property for the 1-to-1 relationship
    public User User { get; set; }
}

public enum Role { Customer, Admin }