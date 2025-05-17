using Dashboard.Data;
using Dashboard.Models;
using Dashboard.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            // 1. Charger toutes les stations
            var stations = await _context.Stations.ToListAsync();
            
            // 2. Charger les derniers paramètres globaux
            var latestParams = await _context.DashboardParams
                .OrderByDescending(b => b.Id)
                .FirstOrDefaultAsync();

            // Normalisation
            double tactTime = Normalize(latestParams?.TactTime ?? 0);
            double conveyorSpeed = Normalize(latestParams?.ConveyorSpeed ?? 0);
            double cycleTime = Normalize(latestParams?.CycleTime ?? 0);
            double actualOutput = Normalize(latestParams?.ActualOutput ?? 0);
            double workingTime = Normalize(latestParams?.WorkingTime ?? 0);
            double targetQuantity = Normalize(latestParams?.TargetQuantity ?? 0);

            // 3. Calculs des indicateurs
            double totalGum = Normalize(stations.Sum(s => s.GumValue));
            double totalAwt = Normalize(stations.Sum(s => s.AwtValue));
            double totalManpower = Normalize(stations.Sum(s => s.DirectOperator + s.IndirectOperator));

            var (pourcentage, donutColor, evalAWTvsGUM) = EvaluerAwtVsGum(totalAwt, totalGum);

            double manpowerNeeded = (tactTime > 0) ? Normalize(totalAwt / tactTime) : 1;
            double manpowerAllocation = (manpowerNeeded > 0) ? Math.Round(totalManpower / manpowerNeeded * 100, 2) : 0;
            var (manpowerColor, manpowerEval) = EvaluerManpowerAllocation(manpowerAllocation);

            double totalAbsDiff = stations.Sum(s => Math.Abs(cycleTime - conveyorSpeed));
            double lineEffectiveness = 0;
            if (totalManpower > 0 && conveyorSpeed > 0)
            {
                lineEffectiveness = Math.Round((totalAbsDiff / (totalManpower * conveyorSpeed)) * 100, 2);
            }
            var (leColor, leEval) = EvaluerLineEffectiveness(lineEffectiveness);

            // 4. Retourner le ViewModel complet
            var dashboardViewModel = new DashboardViewModel
            {
                Stations = stations,
                TotalGum = totalGum,
                TotalAwt = totalAwt,
                PourcentageAWTvsGUM = pourcentage,
                DonutColor = donutColor,
                EvaluationLabel = evalAWTvsGUM,

                TotalManpower = totalManpower,
                ManpowerNeeded = manpowerNeeded,
                ManpowerAllocation = manpowerAllocation,
                ManpowerColor = manpowerColor,
                ManpowerEvaluation = manpowerEval,

                LineEffectiveness = lineEffectiveness,
                LineEffectivenessColor = leColor,
                LineEffectivenessEvaluation = leEval,
                DashboardParams = new DashboardParamViewModel
                 {
                     TactTime = tactTime,
                     ConveyorSpeed = conveyorSpeed,
                     CycleTime = cycleTime,
                     ActualOutput = actualOutput,
                     WorkingTime = workingTime,
                     TargetQuantity = targetQuantity
                 },
            };
            return dashboardViewModel;
        }

        private (double, string, string) EvaluerAwtVsGum(double totalAwt, double totalGum)
        {
            double pourcentage = (totalGum > 0) ? Math.Round((totalAwt / totalGum) * 100, 2) : 0;

            if (pourcentage < 90)
                return (pourcentage, "rgba(255, 206, 86, 0.7)", "Bad");
            else if (pourcentage < 100)
                return (pourcentage, "rgba(75, 192, 192, 0.7)", "Good");
            else
                return (pourcentage, "rgba(255, 99, 132, 0.7)", "Bad");
        }

        private (string, string) EvaluerManpowerAllocation(double pourcentage)
        {
            if (pourcentage < 90)
                return ("rgba(255, 206, 86, 0.7)", "Bad");
            else if (pourcentage <= 110)
                return ("rgba(75, 192, 192, 0.7)", "Good");
            else
                return ("rgba(255, 99, 132, 0.7)", "Bad");
        }

        private (string, string) EvaluerLineEffectiveness(double pourcentage)
        {
            if (pourcentage < 15)
                return ("rgba(75, 192, 192, 0.7)", "Good");
            else
                return ("rgba(255, 99, 132, 0.7)", "Bad");
        }

        private double Normalize(double value)
        {
            return double.IsNaN(value) || double.IsInfinity(value) ? 0 : value;
        }

        private DashboardViewModel GetDefaultDashboard()
        {
            return new DashboardViewModel
            {
                TotalGum = 1,
                TotalAwt = 1,
                PourcentageAWTvsGUM = 0,
                DonutColor = "rgba(255, 99, 132, 0.7)",
                EvaluationLabel = "Aucune station disponible",
                Stations = new List<Station>()
            };
        }
    }

}