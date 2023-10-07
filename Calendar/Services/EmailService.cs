using Calendar.Interfaces;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;

namespace Calendar.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Creates the body and message, then connects to the SMTP server, authenticates the user and sends the message
        public async Task SendEmail(string receiver, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(receiver))
                throw new ArgumentNullException(nameof(receiver));

            if (string.IsNullOrWhiteSpace(subject))
                throw new ArgumentNullException(nameof(subject));

            if (string.IsNullOrWhiteSpace(body))
                throw new ArgumentNullException(nameof(body));

            var builder = CreateBuilder(body);

            var message = CreateMessage(receiver, subject, builder);

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(_configuration["SMTP:Host"], _configuration.GetSection("SMTP:Port").Get<int>(), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration["SMTP:Sender"], _configuration["SMTP:SenderPassword"]);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }

        // Builds the body
        private BodyBuilder CreateBuilder(string body)
        {
            var builder = new BodyBuilder();

            builder.TextBody = body;

            return builder;
        }

        // Builds the message made up of the sender, receiver, subject and body
        private MimeMessage CreateMessage(string receiver, string subject, BodyBuilder builder)
        {
            var message = new MimeMessage();

            message.From.Add(MailboxAddress.Parse(_configuration["SMTP:Sender"]));
            message.To.Add(MailboxAddress.Parse(receiver));
            message.Subject = subject;
            message.Body = builder.ToMessageBody();

            return message;
        }
    }
}
