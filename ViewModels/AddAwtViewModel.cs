using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels
{
    public class AddAwtViewModel
    {
        public int StationId { get; set; }
        [Required]
        public double Value { get; set; }
    }
}
