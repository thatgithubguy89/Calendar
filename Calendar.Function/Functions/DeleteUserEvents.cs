using Calendar.Function.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Calendar.Function.Functions
{
    public class DeleteUserEvents
    {
        private readonly ILogger _logger;

        public DeleteUserEvents(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DeleteUserEvents>();
        }

        // Runs at 1:00 AM every day
        [Function("DeleteUserEvents")]
        public void Run([TimerTrigger("0 1 * * *")] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");

            var http = new HttpClient();
            var deleteUserEventsUrl = Environment.GetEnvironmentVariable("DeleteUserEventsUrl", EnvironmentVariableTarget.Process);

            http.DeleteAsync(deleteUserEventsUrl);
        }
    }
}
