using Calendar.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Calendar.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserEventsController : ControllerBase
    {
        private readonly IUserEventService _userEventService;

        public UserEventsController(IUserEventService userEventService)
        {
            _userEventService = userEventService;
        }

        [HttpGet]
        public async Task<ActionResult> DeletePreviousUserEvents()
        {
            await _userEventService.DeleteAllPreviousUserEventsAsync();

            return Ok();
        }
    }
}
