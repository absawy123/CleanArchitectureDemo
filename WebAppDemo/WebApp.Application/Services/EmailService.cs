using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using WebApp.Application.Interfaces;

namespace WebApp.Application.Services
{
    public class EmailService :IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            // Read from appsettings.json directly
            string smtpServer = _configuration["Email:SmtpServer"]!;
            int port = int.Parse(_configuration["Email:Port"]!);
            string username = _configuration["Email:Username"]!;
            string password = _configuration["Email:Password"]!;
            string senderEmail = _configuration["Email:SenderEmail"]!;

            using var client = new SmtpClient(smtpServer, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail),
                Subject = subject,
                Body = body,
            };

            mailMessage.To.Add(toEmail);
            await client.SendMailAsync(mailMessage);

        }


    }
}
