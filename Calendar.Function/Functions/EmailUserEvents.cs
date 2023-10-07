using Calendar.Function.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Calendar.Function.Functions
{
    public class EmailUserEvents
    {
        private readonly ILogger _logger;

        public EmailUserEvents(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<EmailUserEvents>();
        }

        // This runs every hour
        [Function("EmailUserEvents")]
        public void Run([TimerTrigger("0 * * * *")] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");

            var http = new HttpClient();
            var emailUserEventsUrl = Environment.GetEnvironmentVariable("EmailUserEventsUrl", EnvironmentVariableTarget.Process);

            http.GetAsync(emailUserEventsUrl);
        }
    }
}
