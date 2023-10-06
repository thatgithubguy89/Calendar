using Calendar.Models;

namespace Calendar.Interfaces
{
    public interface IUserEventService
    {
        Task<List<UserEvent>> GetUserEventsByUsernameAndDateAsync(DateTime date);
    }
}
