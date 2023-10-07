using Calendar.Interfaces;

namespace Calendar.Services
{
    public class UserEventEmailService : IUserEventEmailService
    {
        private readonly IEmailService _emailService;
        private readonly IUserEventService _userEventService;
        private const string SUBJECT = "Upcoming Event";

        public UserEventEmailService(IEmailService emailService, IUserEventService userEventService)
        {
            _emailService = emailService;
            _userEventService = userEventService;
        }


        // Gets user events that start in the next hour and emails the creator with info about it
        public async Task EmailHourlyUpdates()
        {
            var userEvents = await _userEventService.GetAllUpcomingUserEventsAsync();

            foreach (var userEvent in userEvents)
            {
                if (!string.IsNullOrWhiteSpace(userEvent.CreatedBy)
                    && !string.IsNullOrWhiteSpace(userEvent.Title)
                    && userEvent.StartTime != null)
                {
                    var receiver = userEvent.CreatedBy;
                    var body = $"Your event {userEvent.Title}\n " +
                               $"will start at {userEvent.StartTime.Value.ToShortTimeString()}";

                    await _emailService.SendEmail(receiver, SUBJECT, body);
                }
            }
        }
    }
}
