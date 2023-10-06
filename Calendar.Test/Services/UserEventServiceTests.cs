using Calendar.Data;
using Calendar.Interfaces;
using Calendar.Models;
using Calendar.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Calendar.Test.Services
{
    [Collection("Sequential")]
    public class UserEventServiceTests : IDisposable
    {
        IUserEventService _userEventService;
        private readonly IMongoCollection<UserEvent> _userEventsCollection;

        private static UserEvent _mockUserEvent = new UserEvent
        {
            Title = "test",
            StartTime = new DateTime(2023, 01, 01, 1, 0, 0, DateTimeKind.Utc),
            CreatedBy = "test@gmail.com"
        };

        private static List<UserEvent> _mockUserEvents = new List<UserEvent>
        {
            new UserEvent {Title = "test", StartTime = DateTime.Now, CreatedBy = "test@gmail.com"},
            new UserEvent {Title = "test", StartTime = DateTime.Now, CreatedBy = "test@gmail.com"}
        };

        private static List<UserEvent> _mockPreviousUserEvents = new List<UserEvent>
        {
            new UserEvent {Title = "test", StartTime = DateTime.Now.AddDays(-1), CreatedBy = "test@gmail.com"},
            new UserEvent {Title = "test", StartTime = DateTime.Now.AddDays(-2), CreatedBy = "test@gmail.com"}
        };

        public UserEventServiceTests()
        {
            var configuration = new ConfigurationBuilder()
            .AddUserSecrets("eb9a0411-27fc-4418-a1f1-9dca6800b66a")
            .Build();

            var mySettings = Options.Create(new CalendarDatabaseSettings
            {
                ConnectionString = configuration["TestCalendarDatabase:ConnectionString"],
                DatabaseName = configuration["TestCalendarDatabase:DatabaseName"],
                UserEventsCollectionName = configuration["TestCalendarDatabase:UserEventsCollectionName"]
            });

            var mongoClient = new MongoClient(configuration["TestCalendarDatabase:ConnectionString"]);
            var mongoDatabase = mongoClient.GetDatabase(configuration["TestCalendarDatabase:DatabaseName"]);
            _userEventsCollection = mongoDatabase.GetCollection<UserEvent>(configuration["TestCalendarDatabase:UserEventsCollectionName"]);

            _userEventService = new UserEventService(mySettings);
        }

        public void Dispose()
        {
            var filter = Builders<UserEvent>.Filter.Empty;
            _userEventsCollection.DeleteMany(filter);
        }

        [Fact]
        public async Task AddUserEventAsync()
        {
            await _userEventService.AddUserEventAsync(_mockUserEvent);
            var userEvents = await _userEventsCollection.Find(_ => true).ToListAsync();
            var result = userEvents.First();

            Assert.IsType<UserEvent>(result);
            Assert.Equal(_mockUserEvent.CreatedBy, result.CreatedBy);
            Assert.Equal(_mockUserEvent.StartTime, result.StartTime);
            Assert.Equal(_mockUserEvent.StartTime.Value.AddHours(1), result.EndTime);
        }

        [Fact]
        public async Task AddUserEventAsync_GivenInvalidUserEvent_Throws_ArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _userEventService.AddUserEventAsync(null));
        }

        [Fact]
        public async Task DeleteAllPreviousUserEventsAsync()
        {
            await _userEventsCollection.InsertManyAsync(_mockPreviousUserEvents);

            await _userEventService.DeleteAllPreviousUserEventsAsync();
            var result = await _userEventsCollection.Find(_ => true).ToListAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task DeleteUserEventAsync()
        {
            await _userEventService.AddUserEventAsync(_mockUserEvent);

            var userEvents = await _userEventsCollection.Find(_ => true).ToListAsync();
            var @event = userEvents.First();
            await _userEventService.DeleteUserEventAsync(@event.Id);
            var result = await _userEventsCollection.Find(x => x.Id == @event.Id).FirstOrDefaultAsync();

            Assert.Null(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task DeleteUserEventAsync_GivenInvalidId_Throws_ArgumentNullException(string id)
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _userEventService.DeleteUserEventAsync(id));
        }

        [Fact]
        public async Task GetUserEventsByUsernameAndDateAsync()
        {
            await _userEventsCollection.InsertManyAsync(_mockUserEvents);

            var result = await _userEventService.GetUserEventsByUsernameAndDateAsync("test@gmail.com", DateTime.Now);

            Assert.IsType<List<UserEvent>>(result);
            Assert.Equal(_mockUserEvents.Count, result.Count);
            Assert.True(_mockUserEvents.All(e => e.CreatedBy == "test@gmail.com"));
            Assert.True(_mockUserEvents.All(e => e.StartTime.Value.Date == DateTime.Now.Date));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task GetUserEventsByUsernameAndDateAsync_GivenInvalidUsername_Throws_ArgumentNullException(string username)
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _userEventService.GetUserEventsByUsernameAndDateAsync(username, DateTime.Now));
        }
    }
}
