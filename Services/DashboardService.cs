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

        public async Task<DashboardViewModel> GetDashboardDataAsync(int lineId)
        {
            var line = await _context.Lines
               .Include(l => l.Stations)
               .FirstOrDefaultAsync(l => l.Id == lineId);

            if (line == null || !line.Stations.Any())
                return GetDefaultDashboard();

            var tactTime = line.TactTime;
            var conveyorSpeed = line.ConveyorSpeed;
            var stations = line.Stations.ToList();

            double totalGum = Normalize(stations.Sum(s => s.GumValue ?? 0));
            double totalAwt = Normalize(stations.Sum(s => s.AwtValue ?? 0));
            double totalManpower = Normalize(stations.Sum(s => (s.DirectOperator ?? 0) + (s.IndirectOperator ?? 0)));

            // KPI 1: AWT vs GUM
            var (pourcentage, couleur, evaluation) = EvaluerAwtVsGum(totalAwt, totalGum);

            // KPI 2: Manpower Allocation
            double manpowerNeeded = (tactTime > 0) ? Normalize(totalAwt / tactTime) : 1;
            double manpowerAllocation = (manpowerNeeded > 0) ? Math.Round(totalManpower / manpowerNeeded * 100, 2) : 0;
            var (colorMP, evalMP) = EvaluerManpowerAllocation(manpowerAllocation);

            // KPI 3: Line Effectiveness
            double totalAbsDifference = stations
                .Where(s => s.AwtValue.HasValue)
                .Sum(s => Math.Abs(s.AwtValue.Value - conveyorSpeed));

            int totalOperators = (int)stations.Sum(s => (s.DirectOperator ?? 0) + (s.IndirectOperator ?? 0));

            double lineEffectiveness = 0;
            if (totalOperators > 0 && conveyorSpeed > 0)
            {
                lineEffectiveness = Math.Round((totalAbsDifference / (totalOperators * line.ConveyorSpeed)) * 100, 2);
            }
            var (colorLE, evalLE) = EvaluerLineEffectiveness(lineEffectiveness);

            return new DashboardViewModel
            {
                Stations = stations,
                TotalGum = totalGum,
                TotalAwt = totalAwt,
                PourcentageAWTvsGUM = pourcentage,
                DonutColor = couleur,
                EvaluationLabel = evaluation,

                // KPI 2
                TotalManpower = totalManpower,
                ManpowerNeeded = manpowerNeeded,
                ManpowerAllocation = manpowerAllocation,
                ManpowerColor = colorMP,
                ManpowerEvaluation = evalMP,
                TactTime = tactTime,
                ConveyorSpeed = conveyorSpeed,

                // KPI 3
                LineEffectiveness = lineEffectiveness,
                LineEffectivenessColor = colorLE,
                LineEffectivenessEvaluation = evalLE
            };
        }
        

        private (double pourcentage, string couleur, string evaluation) EvaluerAwtVsGum(double totalAwt, double totalGum)
        {
            double pourcentage = (totalGum != 0)
                ? Math.Round((totalAwt / totalGum) * 100, 2)
                : 0;

            string couleur;
            string evaluation;

            if (pourcentage < 90)
            {
                couleur = "rgba(255, 206, 86, 0.7)"; // Jaune
                evaluation = "Bad";
            }
            else if (pourcentage < 100)
            {
                couleur = "rgba(75, 192, 192, 0.7)"; // Vert
                evaluation = "Good";
            }
            else
            {
                couleur = "rgba(255, 99, 132, 0.7)"; // Rouge
                evaluation = "Bad";
            }

            return (pourcentage, couleur, evaluation);
        }

        private (string couleur, string evaluation) EvaluerManpowerAllocation(double pourcentage)
        {
            if (pourcentage < 90)
                return ("rgba(255, 206, 86, 0.7)", "Bad"); // Jaune
            else if (pourcentage <= 110)
                return ("rgba(75, 192, 192, 0.7)", "Good"); // Vert
            else
                return ("rgba(255, 99, 132, 0.7)", "Bad"); // Rouge
        }

        private (string couleur, string evaluation) EvaluerLineEffectiveness(double pourcentage)
        {
            if (pourcentage < 15)
                return ("rgba(75, 192, 192, 0.7)", "Good"); // Vert
            else
                return ("rgba(255, 99, 132, 0.7)", "Bad"); // Rouge
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
                EvaluationLabel = "Aucune station disponible"
            };
}
    }
}    
   