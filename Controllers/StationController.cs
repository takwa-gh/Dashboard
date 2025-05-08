using Dashboard.Data;
using Dashboard.Models;
using Dashboard.Services;
using Dashboard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Security.Claims;


namespace Dashboard.Controllers
{
    public class StationController : Controller
    {
        private readonly AppDbContext _context;

        public IStationService StationService { get; }

        public StationController(AppDbContext context, IStationService stationService)
        {
            _context = context;
            StationService = stationService;
        }
        public async Task<IActionResult> Stations(string userName)
        {
            var userIdStr = User.FindFirst("UserId")?.Value;
            var FlaguserId = !Guid.TryParse(userIdStr, out Guid userId);
            if (string.IsNullOrEmpty(userIdStr) || FlaguserId )
            {
                TempData["ErrorMessage"] = "Expired session.";
                return RedirectToAction("Login", "Auth");
            }

            if (User.IsInRole("Manager"))
            {
                var stations = await _context.Stations
                    .Where(s => s.UserId == ""+userId)
                    .ToListAsync();

                var stationViewModel = stations.Select(station => new StationViewModel
                {
                    StationId = ""+station.StationId,
                    StationName = station.StationName,
                    GumValue = station.GumValue,
                    AwtValue = station.AwtValue,
                    PartNumber = station.PartNumber,
                    DirectOperator = station.DirectOperator,
                    IndirectOperator = station.IndirectOperator
                }).ToList();

                return View(stationViewModel);
            }

            if (User.IsInRole("Admin"))
            {
                var stationsQuery = _context.Stations
                    .Include(s => s.User)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(userName))
                {
                    var loweredUserName = userName.ToLower();
                    stationsQuery = stationsQuery
                        .Where(s => s.User != null && s.User.UserName != null &&
                                    s.User.UserName.ToLower().Contains(loweredUserName));
                }

                var stations = await stationsQuery
                    .OrderBy(x => Guid.NewGuid())
                    .ToListAsync();

                var stationViewModel = stations.Select(station => new StationViewModel
                {
                    StationId = ""+station.StationId,
                    StationName = station.StationName,
                    GumValue = station.GumValue,
                    AwtValue = station.AwtValue,
                    PartNumber = station.PartNumber,
                    DirectOperator = station.DirectOperator,
                    IndirectOperator = station.IndirectOperator,

                }).ToList();

                return View(stationViewModel);
            }

            TempData["ErrorMessage"] = "Access denied.";
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public IActionResult CreateStation()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // or "sub" for JWT tokens
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            var lines = _context.Lines
                .Select(line => new LineViewModel
                {
                    Id = line.Id,
                    Name = line.Name
                })
                .ToList();

            var viewModel = new CreateStationPageViewModel
            {
                Station = new CreateStationViewModel() { UserId = userId,Role=userRole },
                Lines = lines
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStation(CreateStationPageViewModel model)
        {



            //if (ModelState.IsValid)
            //{
            //    var station = new Station
            //    {
            //        StationName = model.StationName,
            //        LineId = model.LineId, // Lier à la ligne sélectionnée
            //        UserId = model.UserId
            //    };

            //    _context.Stations.Add(station);
            //    await _context.SaveChangesAsync();
            //    TempData["SuccessMessage"] = "Station created successfully.";
            //    return RedirectToAction("Stations");
            //}

            //// Si le modèle est invalide, renvoyer les lignes disponibles
            //model.Lines = (IEnumerable<System.Web.Mvc.SelectListItem>)_context.Lines
            //    .Select(line => new SelectListItem
            //    {
            //        Value = line.Id.ToString(),
            //        Text = line.Name
            //    }).ToList();


            var flag = await StationService.CreateStationAsync(model.Station, model.Station.UserId, model.Station.Role);
            if (!flag)
            {
                TempData["ErrorMessage"] = "Error while creation";
                return RedirectToAction("stations");
            }
            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var station = await _context.Stations.FindAsync(id);

            if (station == null)
            {
                TempData["ErrorMessage"] = "Station not found.";
                return RedirectToAction("stations");
            }

            var model = new EditStationViewModel
            {
                StationId = station.StationId, // PAS station.StationId.Value
                StationName = station.StationName,
                GumValue = (double)station.GumValue,
                AwtValue = (double)station.AwtValue,
                PartNumber = station.PartNumber,
                DirectOperator = (double)station.DirectOperator,
                IndirectOperator = (double)station.IndirectOperator
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditStationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var station = await _context.Stations.FindAsync(model.StationId);
            if (station == null)
                return NotFound();


            if (User.IsInRole("Admin"))
            {
                TempData["ErrorMessage"] = "Access denied.";
                return RedirectToAction("stations");
            }


            //pour une mise a jour securise et controlee des champs 

            station.StationName = model.StationName;
            station.GumValue = model.GumValue;
            station.AwtValue = model.AwtValue;
            station.PartNumber = model.PartNumber;
            station.DirectOperator = model.DirectOperator;
            station.IndirectOperator = model.IndirectOperator;

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Station updated successfully.";

            return RedirectToAction("stations", "Station");

        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var station = await _context.Stations.FindAsync(id);
            if (station == null)
            {
                return NotFound();
            }
            return View(station);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            // Récupérer l'ID du Manager connecté
            var userIdStr = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out Guid userId))
            {
                TempData["ErrorMessage"] = "Session expired. Please log in again.";
                return RedirectToAction("Login", "Auth");
            }

            // Récupérer la station par son Id
            var station = await _context.Stations.FirstOrDefaultAsync(s => s.StationId == id);
            if (station == null)
            {
                TempData["ErrorMessage"] = "Station not found.";
                return RedirectToAction(nameof(Stations));
            }

            // Suppression de la station
            _context.Stations.Remove(station);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Station deleted successfully.";
            return RedirectToAction(nameof(Stations));
        }


        public async Task<IActionResult> Details(Guid id)
        {
            var station = await _context.Stations.FirstOrDefaultAsync(s => s.StationId == id);
            if (station == null)
            {
                return NotFound();
            }
            return View(station);
        }
    }
}
