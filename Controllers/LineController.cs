using Dashboard.Data;
using Dashboard.Models;
using Dashboard.Services;
using Dashboard.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Dashboard.Controllers
{
    public class LineController : Controller
    {
        private readonly ILineService _lineService;

        public LineController(ILineService lineService)
        {
            _lineService = lineService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateLineViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLineViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _lineService.CreateLineAsync(model);
                TempData["SuccessMessage"] = "Ligne créée avec succès.";
                return RedirectToAction("Index", "Station");
            }

            TempData["ErrorMessage"] = "Veuillez corriger les erreurs.";
            return View(model);
        }
    }

}
