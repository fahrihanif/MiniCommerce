using System.Security.Cryptography;
using Microsoft.AspNetCore.WebUtilities;
using MiniCommerce.API.Abstractions.Handlers;
using MiniCommerce.API.Abstractions.Messages;
using MiniCommerce.API.Contracts;
using MiniCommerce.API.Entities;

namespace MiniCommerce.API.Services.Accounts.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailHandler _emailHandler;

    public RegisterCommandHandler(IAccountRepository accountRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, IEmailHandler emailHandler)
    {
        _accountRepository = accountRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _emailHandler = emailHandler;
    }

    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var verificationToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        
        var account = new Account
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Password = request.Password,
            IsEnabled = true,
            IsActive = false,
            RefreshToken = verificationToken
        };
        
        var user = new User
        {
            AccountId = account.Id,
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Address = request.Address,
            Phone = request.Phone
        };
        
        await _accountRepository.CreateAsync(account, cancellationToken);
        await _userRepository.CreateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        var baseUrl = "https://localhost:7057/api/accounts/verify";
        var queryParams = new Dictionary<string, string?> { { "token", verificationToken } };
        var callbackUrl = QueryHelpers.AddQueryString(baseUrl, queryParams);

        var subject = "Welcome! Please activate your account.";
        var body = $"""
                    <h1>Welcome to Mini Commerce!</h1>
                    <p>Thank you for registering. Please activate your account by clicking the link below:</p>
                    <a href="{callbackUrl}">Activate My Account</a>
                    """;

        await _emailHandler.SendEmailAsync(
            $"{user.FirstName} {user.LastName}",
            account.Email, 
            subject, 
            body);
        
        return Result.Success();
    }
}