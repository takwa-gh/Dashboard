using Dashboard.Services;
using Dashboard.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Dashboard.Controllers
{
    public class StationController : Controller
    {
        private readonly IStationService _stationService;
        

        public StationController(IStationService stationService)
        {
            _stationService = stationService;
            
        }

        public async Task<IActionResult> Stations(string? userName)
        {
            var userIdStr = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out int userId))
            {
                TempData["ErrorMessage"] = "Session expirée.";
                return RedirectToAction("Login", "Auth");
            }

            if (User.IsInRole("Manager"))
            {
                var stations = await _stationService.GetStationsForManagerAsync(userId);
                return View(stations);
            }

            if (User.IsInRole("Admin"))
            {
                var stations = await _stationService.GetStationsForAdminAsync(userName);
                return View(stations);
            }

            TempData["ErrorMessage"] = "Accès refusé.";
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public IActionResult CreateStation()
        {
            var userIdStr = User.FindFirstValue("UserId");
            if (!int.TryParse(userIdStr, out int userId))
            {
                TempData["ErrorMessage"] = "Session expirée.";
                return RedirectToAction("Login", "Auth");
            }

            var viewModel = new CreateStationViewModel
            {
                UserId = userId
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStation(CreateStationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userIdStr = User.FindFirstValue("UserId");
            if (!int.TryParse(userIdStr, out int userId))
            {
                TempData["ErrorMessage"] = "Session expirée.";
                return RedirectToAction("Login", "Auth");
            }

            var flag = await _stationService.CreateStationAsync(model, userId);
            if (!flag)
            {
                TempData["ErrorMessage"] = "Erreur lors de la création.";
                return View(model);
            }

            TempData["SuccessMessage"] = "Station créée avec succès.";
            return RedirectToAction(nameof(Stations));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userIdStr = User.FindFirstValue("UserId");
            if (!int.TryParse(userIdStr, out int userId))
            {
                TempData["ErrorMessage"] = "Session expirée.";
                return RedirectToAction("Login", "Auth");
            }

            var stations = await _stationService.GetStationsForManagerAsync(userId);
            var station = stations.FirstOrDefault(s => s.StationId == id);
            if (station == null)
            {
                TempData["ErrorMessage"] = "Station introuvable.";
                return RedirectToAction(nameof(Stations));
            }

            var model = new EditStationViewModel
            {
                StationId = station.StationId,
                StationName = station.StationName,
                PartNumber = station.PartNumber,
                DirectOperator = (double)station.DirectOperator,
                IndirectOperator = (double)station.IndirectOperator,
                NewAwtValue = (double)station.AwtValue,
                NewGumValue = (double)station.GumValue
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditStationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userIdStr = User.FindFirstValue("UserId");
            if (!int.TryParse(userIdStr, out int userId))
            {
                TempData["ErrorMessage"] = "Session expirée.";
                return RedirectToAction("Login", "Auth");
            }

            if (User.IsInRole("Admin"))
            {
                TempData["ErrorMessage"] = "Accès refusé.";
                return RedirectToAction(nameof(Stations));
            }

            var success = await _stationService.EditStationAsync(model, userId);
            if (!success)
            {
                TempData["ErrorMessage"] = "Erreur lors de la modification.";
                return View(model);
            }

            TempData["SuccessMessage"] = "Station mise à jour avec succès.";
            return RedirectToAction(nameof(Stations));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userIdStr = User.FindFirstValue("UserId");
            if (!int.TryParse(userIdStr, out int userId))
            {
                TempData["ErrorMessage"] = "Session expirée.";
                return RedirectToAction("Login", "Auth");
            }

            var stations = await _stationService.GetStationsForManagerAsync(userId);
            var station = stations.FirstOrDefault(s => s.StationId == id);
            if (station == null)
            {
                TempData["ErrorMessage"] = "Station introuvable.";
                return RedirectToAction(nameof(Stations));
            }

            return View(station);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userIdStr = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out int userId))
            {
                TempData["ErrorMessage"] = "Session expirée.";
                return RedirectToAction("Login", "Auth");
            }

            var success = await _stationService.DeleteStationAsync(id, userId);
            if (!success)
            {
                TempData["ErrorMessage"] = "Erreur lors de la suppression.";
                return RedirectToAction(nameof(Stations));
            }

            TempData["SuccessMessage"] = "Station supprimée avec succès.";
            return RedirectToAction(nameof(Stations));
        }
        [HttpGet]
        public IActionResult AddGUM(int id)
        {
            return View(new AddGumViewModel { StationId = id });
        }

        [HttpPost]
        public async Task<IActionResult> AddGUM(AddGumViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _stationService.AddGUMEntryAsync(model.StationId, model.Value);
            return RedirectToAction("Stations");
        }

        [HttpGet]
        public IActionResult AddAWT(int id)
        {
            return View(new AddAwtViewModel { StationId = id });
        }

        [HttpPost]
        public async Task<IActionResult> AddAWT(AddAwtViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _stationService.AddAWTEntryAsync(model.StationId, model.Value);
            return RedirectToAction("Stations");
        }
        [HttpDelete]
        
        public async Task<IActionResult> DeleteGumEntry(int id)
        {
             await _stationService.DeleteGumEntryAsync(id);

            return RedirectToAction("Stations");
        }

        [HttpDelete]
        [Route("Station/DeleteAwtEntry/{id}")]
        public async Task<IActionResult> DeleteAwtEntry(int id)
        {
            await _stationService.DeleteAwtEntryAsync (id);
            return RedirectToAction("Stations");
        }



    }

}
