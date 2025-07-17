using Microsoft.AspNetCore.Mvc;
using MiniCommerce.API.Common;

namespace MiniCommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IEmailService _emailService;

    public TestController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("send-test-email")]
    public async Task<IActionResult> SendTestEmail()
    {
        var subject = "Hello from MailKit!";
        var body = "<h1>Test Email</h1><p>This is a test email sent from our .NET application and captured by smtp4dev.</p>";

        await _emailService.SendEmailAsync("test.recipient@example.com", subject, body);

        return Ok("Test email sent successfully! Check smtp4dev.");
    }
}