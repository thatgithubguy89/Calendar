using Calendar.Models;

namespace Calendar.Interfaces
{
    public interface IUserEventService
    {
        Task AddUserEventAsync(UserEvent userEvent);
        Task DeleteAllPreviousUserEventsAsync();
        Task DeleteUserEventAsync(string id);
        Task<List<UserEvent>> GetAllUpcomingUserEventsAsync();
        Task<List<UserEvent>> GetUserEventsByUsernameAndDateAsync(string username, DateTime date);
    }
}
