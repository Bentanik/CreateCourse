using Server.Application.Interfaces;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Server.Contracts.Settings;
using Server.Contracts.DTOs.Authentication;

namespace Server.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly EmailSetting _emailConfig;

    public EmailService(IOptions<EmailSetting> emailConfig)
    {
        _emailConfig = emailConfig.Value;
    }


    public async Task<bool> SendMailRegister(EmailRegisterDTO request)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_emailConfig.EmailUsername));
        email.To.Add(MailboxAddress.Parse(request.To));
        email.Subject = request.Subject;
        email.Body = new TextPart(TextFormat.Html)
        {
            Text = request.Body
        };

        try
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailConfig.EmailHost, 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailConfig.EmailUsername, _emailConfig.EmailPassword);
                await client.SendAsync(email);
                await client.DisconnectAsync(true);
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send email: {ex.Message}");
            return false;
        }
    }
}
