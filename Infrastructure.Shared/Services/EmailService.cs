using Application.DTOs.Email;
using Application.Exceptions;
using Application.Interfaces;

using Domain.Settings;

using MailKit.Net.Smtp;
using MailKit.Security;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MimeKit;

using System.Threading.Tasks;

namespace Infrastructure.Shared.Services;

public class EmailService : IEmailService
{
    public MailSettings _mailSettings { get; }
    public ILogger<EmailService> _logger { get; }

    public EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger)
    {
        _mailSettings = mailSettings.Value;
        _logger = logger;
    }

    public async Task SendAsync(EmailRequest request)
    {
        try
        {
            // create message
            MimeMessage email = new()
            {
                Sender = MailboxAddress.Parse(request.From ?? _mailSettings.EmailFrom)
            };
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            BodyBuilder builder = new()
            {
                HtmlBody = request.Body
            };
            email.Body = builder.ToMessageBody();
            using SmtpClient smtp = new();
            smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);

        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw new ApiException(ex.Message);
        }
    }
}
