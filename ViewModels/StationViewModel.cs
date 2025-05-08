using Dashboard.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Dashboard.ViewModels
{
    public class StationViewModel
    {
        public string StationId { get; set; }

        public string StationName { get; set; } = string.Empty;
        public double? GumValue { get; set; }
        public double? AwtValue { get; set; }
       
        public string? PartNumber { get; set; } = string.Empty;

        public double? DirectOperator { get; set; }
        public double? IndirectOperator { get; set; }

        public string UserId { get; set; } // Foreign key to User
        [ValidateNever]
        public User User { get; set; }
    }
}
