using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MiniCommerce.API.Abstractions.Handlers;
using MiniCommerce.API.Abstractions.Messages;
using MiniCommerce.API.Data;

namespace MiniCommerce.API.Services.Accounts.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    private readonly ApplicationDbContext  _context;
    private readonly IJwtTokenHandler _tokenHandler;

    public LoginCommandHandler(ApplicationDbContext context, IJwtTokenHandler jwtTokenHandler)
    {
        _context = context;
        _tokenHandler = jwtTokenHandler;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .SingleOrDefaultAsync(a => 
                a.Email == request.Email);

        if (account == null)
            return Result.Failure<LoginResponse>(AccountErrors.Invalid);

        if (account.IsActive == false)
            return Result.Failure<LoginResponse>(AccountErrors.Verify);
        
        if (account.IsEnabled == false)
            return Result.Failure<LoginResponse>(AccountErrors.Invalid);
        
        if (!BCryptHandler.VerifyPassword(request.Password, account.Password))
            return Result.Failure<LoginResponse>(AccountErrors.Invalid);

        var user = await _context.Users
            .Where(u => u.AccountId == account.Id)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (user == null)
            return Result.Failure<LoginResponse>(AccountErrors.Invalid);
        
        // var token = _tokenHandler.GenerateJwtToken(
        //     user.FirstName, 
        //     user.LastName, 
        //     account.Email, 
        //     account.Role.ToString());
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Email, account.Email),
            new Claim(ClaimTypes.Role, account.Role.ToString()),
        };
        
        var token = _tokenHandler.GenerateJwtToken(claims);
        
        return Result.Success(new LoginResponse(token));
    }
}