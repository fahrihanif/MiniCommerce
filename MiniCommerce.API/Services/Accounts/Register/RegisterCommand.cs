using MiniCommerce.API.Abstractions.Messages;

namespace MiniCommerce.API.Services.Accounts.Register;

public record RegisterCommand(
    string FirstName, 
    string LastName,
    string Phone,
    string Address,
    string Email,
    string Password) : ICommand;
    
    
    