using Calendar.Models;

namespace Calendar.Interfaces
{
    public interface IUserEventService
    {
        Task<List<UserEvent>> GetUserEventsByUsernameAndDateAsync(string username, DateTime date);
    }
}
