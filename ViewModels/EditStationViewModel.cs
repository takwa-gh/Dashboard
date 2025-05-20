using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels
{
    public class EditStationViewModel
    {
        public int StationId { get; set; }

        [Required]
        public string StationName { get; set; } = string.Empty;
        [Display(Name = "Add GUM Value")]
        public double NewGumValue { get; set; }
        [Display(Name = "Add AWT Value")]
        public double NewAwtValue { get; set; }
        
        public string PartNumber { get; set; } = string.Empty;
        public double DirectOperator { get; set; }
        public double IndirectOperator { get; set; }

       

    }

}
