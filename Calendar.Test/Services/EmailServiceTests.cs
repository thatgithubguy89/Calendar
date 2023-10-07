using Calendar.Interfaces;
using Calendar.Services;
using Microsoft.Extensions.Configuration;

namespace Calendar.Test.Services
{
    public class EmailServiceTests
    {
        IEmailService _emailService;
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddUserSecrets("eb9a0411-27fc-4418-a1f1-9dca6800b66a")
            .Build();

        public EmailServiceTests()
        {
            _emailService = new EmailService(configuration);
        }

        [Theory]
        [InlineData(null, "test", "test")]
        [InlineData("", "test", "test")]
        [InlineData(" ", "test", "test")]
        [InlineData("test", null, "test")]
        [InlineData("test", "", "test")]
        [InlineData("test", " ", "test")]
        [InlineData("test", "test", null)]
        [InlineData("test", "test", "")]
        [InlineData("test", "test", " ")]
        public async Task SendEmail_GivenInvalidReceiverSubjectBody_Throws_ArgumentNullException(string receiver, string subject, string body)
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _emailService.SendEmail(receiver, subject, body));
        }

        // Only unskip if sendemail method is changed
        [Fact(Skip = "")]
        public async Task SendEmail()
        {
            //await _emailService.SendEmail(configuration["SMTP:Sender"], "test", "test");
        }
    }
}
