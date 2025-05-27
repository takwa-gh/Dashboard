using Dashboard.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dashboard.Views.Shared.Components
{
    public class DashboardHeaderViewComponent : ViewComponent
    {
        private readonly IDashboardService _dashboardService;

        public DashboardHeaderViewComponent(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model =  _dashboardService.GetDashboardHeader(); // méthode asynchrone à implémenter dans le service
            return View(model);
        }
    }
}
