namespace Dashboard.Models
{
    public class Line
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Attributs globaux utiles pour les KPIs
        public double TactTime { get; set; }
        public double ConveyorSpeed { get; set; }
        public double TargetQuantity { get; set; }
        public double WorkingTime { get; set; }
        public double ActualOutput { get; set; }
        public double CycleTime { get; set; }
        // Navigation
        public ICollection<Station> Stations { get; set; } = new List<Station>();
    }
}
