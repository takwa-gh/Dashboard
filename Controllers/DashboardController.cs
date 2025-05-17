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
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //injection d'entete
            ViewBag.Header = _headerService.GetHeaderData();
            //recuperation des kpis
            var model = await _dashboardService.GetDashboardDataAsync();
            if (!model.HasData)
            {
                ViewBag.Message = "Aucune station disponible.";
            }
            return View(model);
        }
    }
}

   

