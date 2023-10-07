using Calendar.Interfaces;
using Calendar.Models;
using Calendar.Services;
using Moq;

namespace Calendar.Test.Services
{
    public class UserEventEmailServiceTests
    {
        IUserEventEmailService _userEventEmailService;
        Mock<IEmailService> _mockEmailService;
        Mock<IUserEventService> _mockUserEventService;

        private static List<UserEvent> _mockUserEvents = new List<UserEvent>
        {
            new UserEvent {Title = "test", StartTime = DateTime.Now, CreatedBy = "test@gmail.com"},
            new UserEvent {Title = "test", StartTime = DateTime.Now, CreatedBy = "test@gmail.com"}
        };

        public UserEventEmailServiceTests()
        {
            _mockEmailService = new Mock<IEmailService>();
            _mockEmailService.Setup(em => em.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            _mockUserEventService = new Mock<IUserEventService>();
            _mockUserEventService.Setup(e => e.GetAllUpcomingUserEventsAsync()).Returns(Task.FromResult(_mockUserEvents));

            _userEventEmailService = new UserEventEmailService(_mockEmailService.Object, _mockUserEventService.Object);
        }

        // TODO: Setup methods are being ignored and real methods are being called
        [Fact(Skip = "")]
        public async Task EmailHourlyUpdates()
        {
            await _userEventEmailService.EmailHourlyUpdates();
        }
    }
}
