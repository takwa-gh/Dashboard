namespace Dashboard.ViewModels
{
    public class CreateStationPageViewModel
    {
        public CreateStationViewModel Station { get; set; } = new CreateStationViewModel();
        public IEnumerable<LineViewModel> Lines { get; set; } = new List<LineViewModel>();
    }
}
