using MiniCommerce.API.Abstractions.Messages;

namespace MiniCommerce.API.Services.Accounts.Login;

public record LoginCommand(string Email, string Password) : ICommand<LoginResponse>
{};