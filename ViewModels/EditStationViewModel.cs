using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels
{
    public class EditStationViewModel
    {
        public Guid StationId { get; set; }

        [Required]
        public string StationName { get; set; }
        public double GumValue { get; set; }
        public double AwtValue { get; set; }
        
        public string PartNumber { get; set; }
        public double DirectOperator { get; set; }
        public double IndirectOperator { get; set; }
    }

}
