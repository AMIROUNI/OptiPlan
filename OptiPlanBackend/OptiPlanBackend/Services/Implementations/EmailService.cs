using Microsoft.Extensions.Options;
using OptiPlanBackend.Services.Interfaces;
using OptiPlanBackend.Settings;
using System.Net;
using System.Net.Mail;

namespace OptiPlanBackend.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpOptions)
        {
            _smtpSettings = smtpOptions.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_smtpSettings.Email, "OptiPlan"),
                Subject = subject,
                Body = htmlContent,
                IsBodyHtml = true // ⬅️ obligatoire pour que le HTML soit interprété
            };

            message.To.Add(toEmail);

            using (var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
            {
                client.Credentials = new NetworkCredential(_smtpSettings.Email, _smtpSettings.Password);
                client.EnableSsl = true;

                await client.SendMailAsync(message);
            }
        }
    }
}
