using Microsoft.EntityFrameworkCore;
using MiniCommerce.API.Abstractions.Messages;
using MiniCommerce.API.Contracts;
using MiniCommerce.API.Data;

namespace MiniCommerce.API.Services.Accounts.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    private readonly ApplicationDbContext  _context;

    public LoginCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .SingleOrDefaultAsync(a => 
                a.Email == request.Email && a.Password == request.Password, 
                cancellationToken);

        if (account == null)
            return Result.Failure<LoginResponse>(AccountErrors.Invalid);

        if (account.IsActive == false)
            return Result.Failure<LoginResponse>(AccountErrors.Verify);
        
        if (account.IsEnabled == false)
            return Result.Failure<LoginResponse>(AccountErrors.Invalid);
        
        return Result.Success(new LoginResponse("token"));
    }
}