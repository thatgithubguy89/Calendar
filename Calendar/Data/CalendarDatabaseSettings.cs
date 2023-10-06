namespace Calendar.Data
{
    public class CalendarDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string UserEventsCollectionName { get; set; } = null!;
    }
}
