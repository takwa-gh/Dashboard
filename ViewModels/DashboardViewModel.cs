using Dashboard.Models;

namespace Dashboard.ViewModels
{
    public class DashboardViewModel
    {
        public List<Station> Stations { get; set; } = new();
        public double TotalGum { get; set; }  // Total de GumValue
        public double TotalAwt { get; set; }  // Total de AwtValue
        public int AwtGreater { get; set; }  // Nombre de stations où AWT > Gum
        public int AwtBetween { get; set; }  // Nombre de stations où AWT est entre Gum et 0.9 * Gum
        public int AwtLess { get; set; }  // Nombre de stations où AWT < 0.9 * Gum

        public string ComparisonResult { get; set; }
        public double Manpower { get; set; }
        public double Effectiveness { get; set; }


        public double PourcentageAWTvsGUM { get; set; }
        public string DonutColor { get; set; }

        public LineEvaluationViewModel LineEvaluation { get; set; }=new();

    }
}
