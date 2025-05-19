using Dashboard.Services;
using Dashboard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dashboard.Controllers
{
    public class DashboardParamController : Controller
    {
        private readonly IDashboardParamService _service;

        public DashboardParamController(IDashboardParamService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> EditParams()
        {
            var model = await _service.GetDashboardParamsAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditParams(DashboardParamViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // En cas d'erreur de validation, on revient à la vue avec les erreurs affichées
                return View(model);
            }

            if (User.IsInRole("Admin"))
            {
                await _service.UpdateDashboardHeaderAsync(model.DashboardHeader);
            }
            else if (User.IsInRole("Manager"))
            {
                await _service.UpdateDashboardInfoAsync(model.DashboardInfo);
            }

            TempData["Success"] = "Les paramètres ont été mis à jour avec succès.";
            return RedirectToAction("Index", "Dashboard"); 
        }
    }
            
}

