using Calendar.Models;

namespace Calendar.Interfaces
{
    public interface IUserEventService
    {
        Task AddUserEventAsync(UserEvent userEvent);
        Task DeleteUserEventAsync(string id);
        Task<List<UserEvent>> GetUserEventsByUsernameAndDateAsync(string username, DateTime date);
    }
}
