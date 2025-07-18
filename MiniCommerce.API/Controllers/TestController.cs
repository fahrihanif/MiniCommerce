using Microsoft.AspNetCore.Mvc;
using MiniCommerce.API.Abstractions.Handlers;

namespace MiniCommerce.API.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IEmailHandler  _emailHandler;

    public TestController(IEmailHandler emailHandler)
    {
        _emailHandler = emailHandler;
    }

    [HttpPost]
    public async Task<IActionResult> SendTestEmail()
    {
        var subject = "Hello from MiniCommerce";
        var body = "<h1>Test email</h1><p>This is a test</p>";
        
        await _emailHandler.SendEmailAsync("Hanif", "hanif@mail.com", subject, body);
        
        return Ok();
    }
}