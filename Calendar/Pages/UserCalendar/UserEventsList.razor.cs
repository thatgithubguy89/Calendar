using Calendar.Models;
using Microsoft.AspNetCore.Components;

namespace Calendar.Pages.UserCalendar
{
    public partial class UserEventsList : ComponentBase
    {
        [Parameter]
        public List<UserEvent> UserEvents { get; set; }

        [Parameter]
        public EventCallback<string> OnDeleteEvent { get; set; }

        // This is used to call the DeleteEvent method on the parent component SingleDay.
        private async Task DeleteEvent(string id)
        {
            await OnDeleteEvent.InvokeAsync(id);
        }
    }
}
