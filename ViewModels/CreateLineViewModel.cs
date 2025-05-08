using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels
{
    public class CreateLineViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public double TactTime { get; set; }

        [Required]
        public double ConveyorSpeed { get; set; }

        [Required]
        public double TargetQuantity { get; set; }

        [Required]
        public double WorkingTime { get; set; }

        [Required]
        public double ActualOutput { get; set; }

        [Required]
        public double CycleTime { get; set; }
    }
}
