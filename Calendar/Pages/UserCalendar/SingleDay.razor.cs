using Calendar.Interfaces;
using Calendar.Models;
using Microsoft.AspNetCore.Components;

namespace Calendar.Pages.UserCalendar
{
    public partial class SingleDay : ComponentBase
    {
        [Inject]
        public IUserEventService UserEventService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string Date { get; set; } = string.Empty;

        private List<UserEvent> userEvents = new List<UserEvent>();
        private string newEventTime = string.Empty;
        private string title = string.Empty;
        private string message = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            DateTime.TryParse(Date, out DateTime newDate);

            userEvents = await UserEventService.GetUserEventsByUsernameAndDateAsync("test@gmail.com", newDate);
        }

        // If time slot is available, create the user event in the database and refresh page. If not, then display an error message.
        private async Task CreateEvent()
        {
            var date = $"{Date} {newEventTime}";
            DateTime.TryParse(date, out DateTime newDate);

            var userEvent = new UserEvent { Title = title, StartTime = newDate, CreatedBy = "test@gmail.com" };

            if (IsTimeAvailable(newDate))
            {
                await UserEventService.AddUserEventAsync(userEvent);

                NavigationManager.NavigateTo($"/usercalendar/singleday/{Date}", true);
            }
            else
            {
                message = "This time slot is not available.";
                await Task.Delay(3000);
                message = string.Empty;
            }
        }

        // Delete the user event in the database, then delete it from the current user events list.
        private async Task DeleteEvent(string id)
        {
            await UserEventService.DeleteUserEventAsync(id);

            var userEvent = userEvents.FirstOrDefault(e => e.Id == id);
            userEvents.Remove(userEvent);
        }

        // Do any of the current events have the same start time.
        private bool IsTimeAvailable(DateTime date)
        {
            return !userEvents.Any(x => x.StartTime == date);
        }
    }
}
