using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Calendar.Function.Functions
{
    public class UserEventsFunction
    {
        private readonly ILogger _logger;

        public UserEventsFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<UserEventsFunction>();
        }

        // Runs at 1:00 AM every day
        [Function("UserEventsFunction")]
        public void Run([TimerTrigger("0 1 * * *")] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");

            var config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

            var http = new HttpClient();

            http.GetAsync(config.GetValue<string>("UserEventsUrl"));
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
