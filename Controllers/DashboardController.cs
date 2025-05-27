using Dashboard.Services;
using Dashboard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;



namespace Dashboard.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly IKpiEvaluationService _kpiEvaluationService;

        public DashboardController(IDashboardService dashboardService, IKpiEvaluationService kpiEvaluationService)
        {
            _dashboardService = dashboardService;
            _kpiEvaluationService = kpiEvaluationService;
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

            model.KpiAlerts = _kpiEvaluationService.EvaluateKpis(model);

            if (!model.HasData)
            {
                ViewBag.Message = "Aucune station disponible.";
            }

            return View(model); // retourne la vue Razor avec les données
        }




    }
}
