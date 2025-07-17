using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MiniCommerce.API.Common;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
}

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromAddress));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        using var client = new SmtpClient();

        // For smtp4dev, we don't need to authenticate or use SSL.
        // This is for local development testing only.
        await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, false);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}