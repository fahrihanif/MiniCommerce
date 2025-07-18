using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCommerce.API.Services.Accounts.Login;
using MiniCommerce.API.Services.Accounts.Register;
using MiniCommerce.API.Services.Accounts.Verify;

namespace MiniCommerce.API.Controllers;

[ApiController]
[Route("api/accounts")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly ISender  _sender;

    public AccountController(ISender sender)
    {
        _sender = sender;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var result = await _sender.Send(command);
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        await _sender.Send(command);
        
        return Ok();
    }

    [AllowAnonymous]
    [HttpGet("verify")]
    public async Task<IActionResult> Verify([FromQuery]string token)
    {
        var command = new VerifyCommand(token);
        var result = await _sender.Send(command);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok();
    }
}