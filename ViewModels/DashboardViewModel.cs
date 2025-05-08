using Dashboard.Models;

namespace Dashboard.ViewModels
{
    public class DashboardViewModel
    {
        // Données générales
        public List<Station> Stations { get; set; } = new List<Station>();

        // KPI 1 : AWT vs GUM
        public double TotalGum { get; set; }
        public double TotalAwt { get; set; }
        public double PourcentageAWTvsGUM { get; set; }
        public string DonutColor { get; set; }
        public string EvaluationLabel { get; set; } // "Bad", "Good", etc.

        // KPI 2 : Manpower Allocation (à venir si nécessaire)
        public double TotalManpower { get; set; }
        public double ManpowerNeeded { get; set; } // total AWT / TactTime
        public double ManpowerAllocation { get; set; } // ratio
        public double TactTime { get; set; } // À définir selon le besoin
        public double ConveyorSpeed { get; set; } // À définir selon le besoin

        // KPI 3 : Line Effectiveness (à venir si nécessaire)
        public double LineEffectiveness { get; internal set; }
        public string LineEffectivenessColor { get; internal set; }
        public string LineEffectivenessEvaluation { get; internal set; }

        // Tu peux aussi ajouter d'autres sections comme l’en-tête ou des messages
        public string Message { get; set; }
        public string ManpowerColor { get; internal set; }
        public string ManpowerEvaluation { get; internal set; }
       
    }
}
