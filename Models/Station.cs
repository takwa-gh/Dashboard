using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Dashboard.Models
{
    public class Station
    {
        public int StationId { get; set; } =new int();
        
        public required string StationName { get; set; }
        public required double GumValue { get; set; }
        public double MinGumValue { get; set; }
        public  double MaxGumValue { get; set; }
        public double AverageGumValue { get; set; }
        public required double AwtValue { get; set; }
        public double MinAwtValue { get; set; }
        public  double MaxAwtValue { get; set; }
        public double AverageAwtValue { get; set; }
        public required string PartNumber { get; set; } = string.Empty;
        
        public required double DirectOperator { get; set; }
        public required double IndirectOperator { get; set; }

        public required int UserId { get; set; } // Foreign key to User
        [ValidateNever]
        public  User User { get; set; }

        // Navigation properties
        public ICollection<StationAWT> AWTEntries { get; set; } = new List<StationAWT>();
        public ICollection<StationGUM> GUMEntries { get; set; } = new List<StationGUM>();

    }
}
