using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class DashboardParam
    {
        public int Id { get; set; }
        public string? Plant { get; set; }
        public string? Project { get; set; }
        public string? Family { get; set; }
        public string? ControlNumber { get; set; }
        // Attributs globaux utiles pour les KPIs
        public double TactTime { get; set; }
        public double ConveyorSpeed { get; set; }
        public double TargetQuantity { get; set; }
        public double WorkingTime { get; set; }
        public double ActualOutput { get; set; }
        public double CycleTime { get; set; }

    }
}
