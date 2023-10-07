using Calendar.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Calendar.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserEventsController : ControllerBase
    {
        private readonly ILogger<UserEventsController> _logger;
        private readonly IUserEventEmailService _userEventEmailService;
        private readonly IUserEventService _userEventService;

        public UserEventsController(ILogger<UserEventsController> logger, IUserEventEmailService userEventEmailService, IUserEventService userEventService)
        {
            _logger = logger;
            _userEventEmailService = userEventEmailService;
            _userEventService = userEventService;
        }

        [HttpGet]
        [Route("email")]
        public async Task<ActionResult> EmailUpcomingUserEvents()
        {
            try
            {
                _logger.LogInformation("Emailing users about their upcoming events");

                await _userEventEmailService.EmailHourlyUpdates();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to email users about their upcoming events: {}", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeletePreviousUserEvents()
        {
            try
            {
                _logger.LogInformation("Deleting previous user events ");

                await _userEventService.DeleteAllPreviousUserEventsAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete previous user events: {}", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
