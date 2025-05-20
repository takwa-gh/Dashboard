using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels
{
    public class AddGumViewModel
    {
        public int StationId { get; set; }
        [Required]
        public double Value { get; set; }
    }
}
