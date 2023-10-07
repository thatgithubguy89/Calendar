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

        public async Task AddUserEventAsync(UserEvent userEvent)
        {
            if (userEvent == null)
                throw new ArgumentNullException(nameof(userEvent));

            userEvent.EndTime = userEvent.StartTime.Value.AddHours(1);
            userEvent.CreateTime = DateTime.Now;
            userEvent.LastEditTime = DateTime.Now;

            await _userEventsCollection.InsertOneAsync(userEvent);
        }

        // Deletes all user events before the current day
        public async Task DeleteAllPreviousUserEventsAsync()
        {
            var filter = Builders<UserEvent>.Filter.Lt(e => e.StartTime, DateTime.Now);

            await _userEventsCollection.DeleteManyAsync(filter);
        }

        public async Task DeleteUserEventAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));

            await _userEventsCollection.DeleteOneAsync(e => e.Id == id);
        }

        // Gets all user events set to start an hour from the current time
        public async Task<List<UserEvent>> GetAllUpcomingUserEventsAsync()
        {
            var userEvents = await _userEventsCollection.Find(_ => true).ToListAsync();

            if (!userEvents.Any())
                return userEvents;

            userEvents = userEvents.Where(e => e.StartTime.Value.Date == DateTime.Now.Date).ToList();
            userEvents = userEvents.Where(e => e.StartTime.Value.Hour == DateTime.Now.ToUniversalTime().AddHours(1).Hour).ToList();

            return userEvents;
        }

        // Get user events for a specific user for one day.
        public async Task<List<UserEvent>> GetUserEventsByUsernameAndDateAsync(string username, DateTime date)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));

            var events = await _userEventsCollection.Find(e => e.CreatedBy == username).ToListAsync();

            return events.Where(e => e.StartTime.Value.Date == date.Date).ToList();
        }
    }
}
