using Microsoft.AspNetCore.Http.HttpResults;
using MiniCommerce.API.Abstractions.Messages;

namespace MiniCommerce.API.Services.Accounts;

public class AccountErrors
{
    public static readonly Error Invalid = new Error(
        "Account.Invalid",
        "Email or Password is incorrect.");
    
    public static readonly Error InvalidToken = new Error(
        "Account.InvalidToken",
        "Invalid verification token.");
    
    public static readonly Error Verify = new Error(
        "Account.Verify", 
        "You need to activate the account, please check you Mailbox.");
}