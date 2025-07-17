using Microsoft.EntityFrameworkCore;
using MiniCommerce.API.Abstractions.Messages;
using MiniCommerce.API.Contracts;
using MiniCommerce.API.Data;

namespace MiniCommerce.API.Services.Accounts.Verify;

public class VerifyCommandHandler : ICommandHandler<VerifyCommand>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;

    public VerifyCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork, ApplicationDbContext context)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<Result> Handle(VerifyCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .Where(a => a.RefreshToken == request.Token)
            .FirstOrDefaultAsync(cancellationToken);

        if (account is null)
        {
            return Result.Failure(AccountErrors.InvalidToken);
        }

        account.IsActive = true;
        account.RefreshToken = null;

        await _accountRepository.UpdateAsync(account);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}