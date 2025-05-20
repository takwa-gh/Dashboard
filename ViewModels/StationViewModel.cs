using Dashboard.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Dashboard.ViewModels
{
    public class StationViewModel
    {
        public int StationId { get; set; }

        public string StationName { get; set; } = string.Empty;

        // Dernières valeurs ajoutées (optionnelles)
        public double? GumValue { get; set; }
        public double? AwtValue { get; set; }

        // Valeurs calculées
        public double MinGumValue { get; set; }
        public double MaxGumValue { get; set; }
        public double AverageGumValue { get; set; }

        public double MinAwtValue { get; set; }
        public double MaxAwtValue { get; set; }
        public double AverageAwtValue { get; set; }

        public string? PartNumber { get; set; } = string.Empty;

        public double? DirectOperator { get; set; }
        public double? IndirectOperator { get; set; }

        public List<StationGUM> GUMEntries { get; set; } = new();
        public List<StationAWT> AWTEntries { get; set; }= new();

        public int UserId { get; set; } // Foreign key to User

        [ValidateNever]
        public User User { get; set; }
    }
}

