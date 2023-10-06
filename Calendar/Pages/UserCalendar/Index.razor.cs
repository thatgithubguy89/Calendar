using Microsoft.AspNetCore.Components;

namespace Calendar.Pages.UserCalendar
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private void OnCellClicked(DateTime date)
        {
            NavigationManager.NavigateTo($"usercalendar/singleday/{date.ToString("yyyy-MM-dd")}");
        }
    }
}
