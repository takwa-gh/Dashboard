using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Dashboard.Models
{
    public class Station
    {
        public Guid StationId { get; set; } = Guid.NewGuid();
        
        public string StationName { get; set; } = string.Empty;
        public double? GumValue { get; set; }
        public double? AwtValue { get; set; }
        public string? PartNumber { get; set; } = string.Empty;
        
        public double? DirectOperator { get; set; }
        public double? IndirectOperator { get; set; }

        public Guid UserId { get; set; } // Foreign key to User
        [ValidateNever]
        public User User { get; set; }

        public int LineId { get; set; }
        public Line Line { get; set; }  // Navigation property
    }
}
