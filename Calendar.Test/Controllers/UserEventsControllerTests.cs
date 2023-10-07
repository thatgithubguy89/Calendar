using Calendar.Controllers;
using Calendar.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Calendar.Test.Controllers
{
    public class UserEventsControllerTests
    {
        Mock<ILogger<UserEventsController>> _mockLogger;
        Mock<IUserEventEmailService> _mockUserEventEmailService;
        Mock<IUserEventService> _mockUserEventService;

        public UserEventsControllerTests()
        {
            _mockLogger = new Mock<ILogger<UserEventsController>>();
            _mockUserEventEmailService = new Mock<IUserEventEmailService>();
            _mockUserEventService = new Mock<IUserEventService>();
        }

        [Fact]
        public async Task EmailUpcomingUserEvents()
        {
            _mockUserEventEmailService.Setup(e => e.EmailHourlyUpdates());
            var _userEventsController = new UserEventsController(_mockLogger.Object, _mockUserEventEmailService.Object, _mockUserEventService.Object);

            var actionResult = await _userEventsController.EmailUpcomingUserEvents();
            var result = actionResult as NoContentResult;

            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Fact]
        public async Task EmailUpcomingUserEvents_Failure_Returns_InternalServerError()
        {
            _mockUserEventEmailService.Setup(e => e.EmailHourlyUpdates()).Throws(new Exception());
            var _userEventsController = new UserEventsController(_mockLogger.Object, _mockUserEventEmailService.Object, _mockUserEventService.Object);

            var actionResult = await _userEventsController.EmailUpcomingUserEvents();
            var result = actionResult as StatusCodeResult;

            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Fact]
        public async Task DeletePreviousUserEvents()
        {
            _mockUserEventService.Setup(e => e.DeleteAllPreviousUserEventsAsync());
            var _userEventsController = new UserEventsController(_mockLogger.Object, _mockUserEventEmailService.Object, _mockUserEventService.Object);

            var actionResult = await _userEventsController.DeletePreviousUserEvents();
            var result = actionResult as NoContentResult;

            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Fact]
        public async Task DeletePreviousUserEvents_Failure_Returns_InternalServerError()
        {
            _mockUserEventService.Setup(e => e.DeleteAllPreviousUserEventsAsync()).Throws(new Exception());
            var _userEventsController = new UserEventsController(_mockLogger.Object, _mockUserEventEmailService.Object, _mockUserEventService.Object);

            var actionResult = await _userEventsController.DeletePreviousUserEvents();
            var result = actionResult as StatusCodeResult;

            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }
    }
}
