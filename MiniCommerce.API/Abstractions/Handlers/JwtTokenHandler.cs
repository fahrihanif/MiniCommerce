using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MiniCommerce.API.Entities;

namespace MiniCommerce.API.Abstractions.Handlers;

public interface IJwtTokenHandler
{
    string GenerateJwtToken(string firstName, string? lastName, string email, string roleName);
    string GenerateJwtToken(IEnumerable<Claim> claims);
}

public class JwtTokenHandler : IJwtTokenHandler
{
    private string _issuer;
    private string _audience;
    private string _key;

    public JwtTokenHandler(string issuer, string audience, string key)
    {
        _issuer = issuer;
        _audience = audience;
        _key = key;
    }

    public string GenerateJwtToken(
        string firstName, string? lastName, string email, string roleName)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, $"{firstName} {lastName}"),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, roleName),
        };
        
        // if user have more than 1 role
        //claims.AddRange(rolesName.Select(name => new Claim(ClaimTypes.Role, name)));

        var key = Encoding.UTF8.GetBytes(_key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(15),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        
        return jwtToken;
    }

    public string GenerateJwtToken(IEnumerable<Claim> claims)
    {
        var key = Encoding.UTF8.GetBytes(_key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(15),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        
        return jwtToken;
    }
}