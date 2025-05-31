using Dashboard.Models;
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
        private readonly IActivityLogService _activityLogService;
        

        public StationController(IStationService stationService , IActivityLogService activityLogService)
        {
            _stationService = stationService;
            _activityLogService = activityLogService;
        }

        public async Task<IActionResult> Stations(string? userName)
        {
            var userIdStr = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out int userId))
            {
                TempData["ErrorMessage"] = "Session expirée.";
                return RedirectToAction("Login", "Auth");
            }

            if (User.IsInRole("TeamLeader"))
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
            await _activityLogService.LogAsync(User.Identity?.Name, $"Created station: {model.StationName}");

            TempData["SuccessMessage"] = "Station created successfully.";
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
                TempData["ErrorMessage"] = "Expired session.";
                return RedirectToAction("Login", "Auth");
            }

            if (User.IsInRole("Admin"))
            {
                TempData["ErrorMessage"] = "Access denied.";
                return RedirectToAction(nameof(Stations));
            }

            var success = await _stationService.EditStationAsync(model, userId);
            if (!success)
            {
                TempData["ErrorMessage"] = "Update failed.";
                return View(model);
            }
            await _activityLogService.LogAsync(User.Identity?.Name, $"Updated station : {model.StationName}");


            TempData["SuccessMessage"] = "Station updated successfully.";
            return RedirectToAction(nameof(Stations));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userIdStr = User.FindFirstValue("UserId");
            if (!int.TryParse(userIdStr, out int userId))
            {
                TempData["ErrorMessage"] = "Expired session.";
                return RedirectToAction("Login", "Auth");
            }

            var stations = await _stationService.GetStationsForManagerAsync(userId);
            var station = stations.FirstOrDefault(s => s.StationId == id);
            if (station == null)
            {
                TempData["ErrorMessage"] = "Station not found.";
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
                TempData["ErrorMessage"] = "Error while deleting.";
                return RedirectToAction(nameof(Stations));
            }
            await _activityLogService.LogAsync(User.Identity?.Name, $"Deleted station : (ID: {id})");

            TempData["SuccessMessage"] = "Station deleted successfully .";
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
            await _activityLogService.LogAsync(User.Identity?.Name,$"Add a GUM value ({model.Value}) to station ID {model.StationId}");

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
            await _activityLogService.LogAsync(User.Identity?.Name,$"Add a AWT value ({model.Value}) to station ID {model.StationId}");

            return RedirectToAction("Stations");
        }
        [HttpDelete]
        
        public async Task<IActionResult> DeleteGumEntry(int id)
        {
             await _stationService.DeleteGumEntryAsync(id);
            await _activityLogService.LogAsync(User.Identity?.Name,$"Delete a GUM value");


            return RedirectToAction("Stations");
        }

        [HttpDelete]
        [Route("Station/DeleteAwtEntry/{id}")]
        public async Task<IActionResult> DeleteAwtEntry(int id)
        {
            await _stationService.DeleteAwtEntryAsync (id);
            await _activityLogService.LogAsync(User.Identity?.Name,$"Delete an AWT value");

            return RedirectToAction("Stations");
        }



    }

}
