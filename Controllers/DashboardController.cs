using Dashboard.Data;
using Dashboard.Models;
using Dashboard.Services;
using Dashboard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dashboard.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly IHeaderService _headerService;

        public DashboardController(IDashboardService dashboardService, IHeaderService headerService)
        {
            _dashboardService = dashboardService;
            _headerService = headerService;
        }

        public async Task<IActionResult> Index(int lineId)
        {
            ViewBag.Header = _headerService.GetHeaderData();
            var model = await _dashboardService.GetDashboardDataAsync(lineId);
            return View(model);
        }
    }
}
   

