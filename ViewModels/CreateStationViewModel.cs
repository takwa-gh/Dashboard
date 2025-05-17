using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels
{
    public class CreateStationViewModel
    {
        public string StationName { get; set; } = string.Empty;
        public double GumValue { get; set; }
        public double AwtValue { get; set; }

        public string PartNumber { get; set; } = string.Empty;

        public double DirectOperator { get; set; }
        public double IndirectOperator { get; set; }

        public int UserId { get; set; } // Foreign key to User
        
    }
}
