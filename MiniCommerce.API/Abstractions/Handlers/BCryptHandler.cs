using BCrypt.Net;

namespace MiniCommerce.API.Abstractions.Handlers;

public class BCryptHandler
{
    private static string GenerateSalt()
    {
        return BCrypt.Net.BCrypt.GenerateSalt(12);
    }
    
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, GenerateSalt());
    }
    
    public static bool VerifyPassword(string inputPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
    }
}