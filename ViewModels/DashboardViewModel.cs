using Dashboard.Models;

namespace Dashboard.ViewModels
{
    public class DashboardViewModel
    {
        // Données générales
        public List<Station> Stations { get; set; } 

        // KPI 1 : AWT vs GUM
        public double TotalGum { get; set; }
        public double TotalAwt { get; set; }
        public double pourcentageAWTvsGUM { get; set; } //(TotalAwt / TotalGum) * 100
       
        

        // KPI 2 : Manpower Allocation (à venir si nécessaire)
        public double TotalManpower { get; set; } //somme de DirectOperator + IndirectOperator
        public double ManpowerNeeded { get; set; } // total AWT / TactTime
        public double ManpowerAllocation { get; set; } // TotalManpower / ManpowerNeeded

        // KPI 3 : Line Effectiveness (à venir si nécessaire)
        public double LineEffectiveness { get; set; } //somme(abs(CycleTime - ConveyorSpeed)) / (TotalManpower * ConveyorSpeed)
        

        // Tu peux aussi ajouter d'autres sections comme l’en-tête ou des messages
       
       

        public bool HasData => Stations != null && Stations.Any();

        // Paramètres saisis manuellement ou enregistrés par un manager

        public LineParamViewModel LineParams { get; set; }

        public List<KpiAlertViewModel> KpiAlerts { get; set; } = new List<KpiAlertViewModel>();



    }
}