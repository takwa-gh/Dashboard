using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels
{
    public class DashboardParamViewModel
    {
        
            [Required]
            [Display(Name = "Conveyor Speed")]
            public double ConveyorSpeed { get; set; }

            [Required]
            [Display(Name = "Tact Time")]
            public double TactTime { get; set; }

            [Required]
            [Display(Name = "Target Quantity")]
            public double TargetQuantity { get; set; }
            [Required]
            [Display(Name = "Working Time")]
            public double WorkingTime{ get; set; }
            [Required]
            [Display(Name = "Actual Output")]
            public double ActualOutput { get; set; }
            [Required]
            [Display(Name = "Cycle Time")]
            public double CycleTime { get; set; }



    }
}
