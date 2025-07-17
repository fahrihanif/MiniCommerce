using MiniCommerce.API.Abstractions.Messages;

namespace MiniCommerce.API.Services.Accounts.Verify;

public record VerifyCommand(string Token) : ICommand;