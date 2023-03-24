using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ProjectFinalEngineer.Services;

public class MailSettings
{
    public string Mail { get; set; }
    public string DisplayName { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }

}

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string message);
}

public class SendMailService : IEmailSender
{

    private readonly MailSettings _mailSettings;

    public SendMailService(IOptions<MailSettings> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage();
        message.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
        message.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
        message.To.Add(MailboxAddress.Parse(email));
        message.Subject = subject;

        var builder = new BodyBuilder
        {
            HtmlBody = htmlMessage
        };
        message.Body = builder.ToMessageBody();

        //SmtpClient của MailKit
        using var smtp = new MailKit.Net.Smtp.SmtpClient();

        try
        {
            await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(message);
        }

        catch (Exception)
        {
            // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
            Directory.CreateDirectory("mailssave");
            var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
            await message.WriteToAsync(emailsavefile);
        }

        await smtp.DisconnectAsync(true);
    }
}