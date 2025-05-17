using Dashboard.Services;
using Dashboard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    [Authorize(Roles = "Manager,Admin")]
    public class DashboardParamController : Controller
    {
        private readonly IDashboardParamService _service;

        public DashboardParamController(IDashboardParamService service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult EditParametres()
        {
            var model = _service.Get();

            // Si l'utilisateur est admin → rendre la vue en lecture seule
            if (User.IsInRole("Admin"))
            {
                ViewBag.IsReadOnly = true;
            }
            else if (User.IsInRole("Manager"))
            {
                ViewBag.IsReadOnly = false;
            }
            
                return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")] // Seuls les managers peuvent modifier
        public IActionResult EditParametres(DashboardParamViewModel model)
        {
            if (User.IsInRole("Admin"))
            {
                return Forbid();
            }
            if (ModelState.IsValid)
            {
                _service.SaveOrUpdate(model);
                return RedirectToAction("Index", "Dashboard"); // Redirection après sauvegarde
            }

            ViewBag.IsReadOnly = false; // Réaffiche le formulaire avec les erreurs
            return View(model);
        }
    }
}
