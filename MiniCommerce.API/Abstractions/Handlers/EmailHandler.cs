using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MiniCommerce.API.Abstractions.Handlers;

public interface IEmailHandler
{
    Task SendEmailAsync(string toName, string toEmail, string subject, string body);
}

public class EmailHandler : IEmailHandler
{
    private readonly EmailSetting _emailSetting;

    public EmailHandler(IOptions<EmailSetting> emailSetting)
    {
        _emailSetting = emailSetting.Value;
    }

    public async Task SendEmailAsync(string toName, string toEmail, string subject, string body)
    {
        var fromMailAddress = new MailboxAddress(_emailSetting.FromName, _emailSetting.FromAddress);
        var toMailAddress = new MailboxAddress(toName,toEmail);
        
        var message = new MimeMessage();
        message.From.Add(fromMailAddress);
        message.To.Add(toMailAddress);
        message.Subject = subject;
        message.Body = new TextPart("html") {Text = body};

        using var client = new SmtpClient();
        await client.ConnectAsync(_emailSetting.Host, _emailSetting.Port, false);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}