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
        private readonly AppDbContext _context;
        private readonly IHeaderService _headerService;

        public DashboardController(AppDbContext context, IHeaderService headerService)
        {
            _context = context;
            _headerService = headerService;
        }


        private double NormalizeDouble(double value)
        {
            return double.IsNaN(value) || double.IsInfinity(value) ? 0 : value;
        }
        public async Task<IActionResult> Index()
        {
            // Récupérer les stations
            var headerData = _headerService.GetHeaderData();
            ViewBag.Header = headerData;
            var stations = await _context.Stations.ToListAsync();

            // Vérifier si la liste est vide
            if (!stations.Any())
            {
                // Si la liste est vide, retourner un modèle avec des valeurs par défaut
                return View(new DashboardViewModel
                {
                    TotalGum = 1,
                    TotalAwt = 1,
                    AwtGreater = 0,
                    AwtBetween = 0,
                    AwtLess = 0,
                    ComparisonResult = "Aucune station disponible",
                    PourcentageAWTvsGUM = 0,
                    DonutColor = "rgba(255, 99, 132, 0.7)", // Couleur par défaut
                    LineEvaluation = new LineEvaluationViewModel
                    {
                        TotalGUM = 1,
                        TotalAWT = 1,
                        Manpower = 0,
                        TactTime = 1 // Valeur par défaut
                    }
                });
            }

            // Si la liste n'est pas vide, procéder aux calculs
            double totalGum = NormalizeDouble(stations.Sum(s => s.GumValue ?? 0));
            double totalAwt = NormalizeDouble(stations.Sum(s => s.AwtValue ?? 0));
            double totalManpower = NormalizeDouble(stations.Sum(s => (s.DirectOperator ?? 0) + (s.IndirectOperator ?? 0)));

           

            int awtGreater = stations.Count(s => s.AwtValue > s.GumValue);
            int awtBetween = stations.Count(s => s.AwtValue <= s.GumValue && s.AwtValue >= 0.9 * s.GumValue);
            int awtLess = stations.Count(s => s.AwtValue < 0.9 * s.GumValue);

            string ComparisonResult = totalAwt > totalGum ? "AWT > Gum" :
                                      totalAwt == totalGum ? "AWT == Gum" : "AWT < Gum";

            double PourcentageAWTvsGUM = (totalGum != 0 && !double.IsNaN(totalAwt) && !double.IsInfinity(totalAwt))
            ? Math.Round((totalAwt / totalGum) * 100, 2)
            : 0;

            string donutColor = totalAwt > totalGum ? "rgba(255, 99, 132, 0.7)" :
                                (totalAwt >= 0.9 * totalGum ? "rgba(75, 192, 192, 0.7)"
                                : "rgba(255, 206, 86, 0.7)");

            // Remplir le modèle avec les résultats calculés
            var model = new DashboardViewModel
            {
                Stations = stations,
                TotalGum = totalGum,
                TotalAwt = totalAwt,
                AwtGreater = awtGreater,
                AwtBetween = awtBetween,
                AwtLess = awtLess,
                ComparisonResult = ComparisonResult,

                LineEvaluation = new LineEvaluationViewModel
                {
                    TotalGUM = totalGum,
                    TotalAWT = totalAwt,
                    Manpower = totalManpower,
                   
                },

                PourcentageAWTvsGUM = PourcentageAWTvsGUM,
                DonutColor = donutColor
            };

            // Retourner la vue avec le modèle
            return View(model);
        }
    }
}
