using GymManagementSystem.Config;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace GymManagementSystem.Services;

public class EmailService(IOptions<EmailConfig> options) : IEmailService
{
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var client = new SendGridClient(options.Value.SendGridApiKey);
        var from = new EmailAddress(options.Value.FromEmail, options.Value.FromName);
        var to = new EmailAddress(email);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
        var response = await client.SendEmailAsync(msg);

        // Optional: check response status
        if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
            throw new Exception("Email sending failed.");
    }
}