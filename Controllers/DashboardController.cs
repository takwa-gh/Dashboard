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
        
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
            
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
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

   

