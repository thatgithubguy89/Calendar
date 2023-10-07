namespace Calendar.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string receiver, string subject, string body);
    }
}
