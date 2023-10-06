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
        private readonly IHttpContextAccessor _http;

        public UserEventService(IOptions<CalendarDatabaseSettings> calendarDatabaseSettings, IHttpContextAccessor http)
        {
            var mongoClient = new MongoClient(calendarDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(calendarDatabaseSettings.Value.DatabaseName);

            _userEventsCollection = mongoDatabase.GetCollection<UserEvent>(calendarDatabaseSettings.Value.UserEventsCollectionName);

            _http = http;
        }

        public async Task<List<UserEvent>> GetUserEventsByUsernameAndDateAsync(DateTime date)
        {
            //var username = _http.HttpContext.User.Claims.ToList()[1].Value;
            var username = "test@gmail.com";

            var events = await _userEventsCollection.Find(e => e.CreatedBy == username).ToListAsync();

            return events.Where(e => e.StartTime.Value.Date == date.Date).ToList();
        }
    }
}
