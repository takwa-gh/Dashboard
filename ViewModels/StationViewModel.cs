using Dashboard.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Dashboard.ViewModels
{
    public class StationViewModel
    {
        public int StationId { get; set; }

        public string StationName { get; set; } = string.Empty;
        public double? GumValue { get; set; }
        public double? AwtValue { get; set; }
       
        public string? PartNumber { get; set; } = string.Empty;

        public double? DirectOperator { get; set; }
        public double? IndirectOperator { get; set; }

        public int UserId { get; set; } // Foreign key to User
        [ValidateNever]
        public User User { get; set; }
    }
}
