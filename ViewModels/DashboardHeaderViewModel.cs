using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels
{
    public class DashboardHeaderViewModel
    {
        [Required]
        [Display(Name = "Plant")]
        public string? Plant { get; set; }
        [Required]
        [Display(Name = "Project")]
        public string? Project { get; set; }
        [Required]
        [Display(Name = "Family")]
        public string? Family { get; set; }
        [Required]
        [Display(Name = "Control Number")]
        public string? ControlNumber { get; set; }

       
    }
}
