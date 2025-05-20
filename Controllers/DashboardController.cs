using Dashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;



namespace Dashboard.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public IActionResult DashboardHeader()
        {
            var model = _dashboardService.GetDashboardHeader();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _dashboardService.GetDashboardDataAsync();
            if (!model.HasData)
            {
                ViewBag.Message = "Aucune station disponible.";
            }
            return View(model);
        }
    }
}
