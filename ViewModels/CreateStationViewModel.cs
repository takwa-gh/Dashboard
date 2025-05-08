using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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

        public string UserId { get; set; } // Foreign key to User
        public string Role { get; set; } // Foreign key to User

        //[Required]
        //[Display(Name = "Line")]
        //public int LineId { get; set; }


        // Liste des lignes disponibles à sélectionner
        public IEnumerable<SelectListItem> Lines { get; set; }

        public int? lineId { get; set; }
        public CreateLineViewModel? line { get; set; }
    }
}
