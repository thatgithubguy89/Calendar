using Calendar.Interfaces;
using Calendar.Models;
using Microsoft.AspNetCore.Components;

namespace Calendar.Pages.UserCalendar
{
    public partial class SingleDay : ComponentBase
    {
        [Inject]
        public IUserEventService UserEventService { get; set; }

        [Parameter]
        public string Date { get; set; } = string.Empty;

        private List<UserEvent> userEvents = new List<UserEvent>();

        protected override async Task OnInitializedAsync()
        {
            DateTime.TryParse(Date, out DateTime newDate);

            userEvents = await UserEventService.GetUserEventsByUsernameAndDateAsync("test@gmail.com", newDate);
        }
    }
}
