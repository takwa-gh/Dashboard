using Dashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Views.Shared.Components
{
    public class DashboardHeaderViewComponent : ViewComponent
    {
        private readonly IDashboardService _dashboardService;

        public DashboardHeaderViewComponent(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public IViewComponentResult Invoke()
        {
            var model = _dashboardService.GetDashboardHeader();
            return View(model);
        }
    }
}