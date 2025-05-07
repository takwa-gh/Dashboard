namespace Dashboard.ViewModels
{
    public class LineEvaluationViewModel
    {
        public double TotalGUM { get; set; }
        public double TotalAWT { get; set; }

        public double Manpower { get; set; } = 1; // Default value to avoid division by zero
        public double TactTime { get; set; }

        public double ManpowerAllocation => TotalGUM / (Manpower * TactTime);
        public double AwtVsGumRatio => TotalAWT / TotalGUM;
        public double LineEffectiveness => TotalAWT / (Manpower * TactTime);
    }

}
