using Calendar.Data;
using Calendar.Interfaces;
using Calendar.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Calendar.Services
{
    public class UserEventService : IUserEventService
    {
        private readonly IMongoCollection<UserEvent> _userEventsCollection;

        public UserEventService(IOptions<CalendarDatabaseSettings> calendarDatabaseSettings)
        {
            var mongoClient = new MongoClient(calendarDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(calendarDatabaseSettings.Value.DatabaseName);

            _userEventsCollection = mongoDatabase.GetCollection<UserEvent>(calendarDatabaseSettings.Value.UserEventsCollectionName);
        }

        public async Task<List<UserEvent>> GetUserEventsByUsernameAndDateAsync(string username, DateTime date)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));

            var events = await _userEventsCollection.Find(e => e.CreatedBy == username).ToListAsync();

            return events.Where(e => e.StartTime.Value.Date == date.Date).ToList();
        }
    }
}
