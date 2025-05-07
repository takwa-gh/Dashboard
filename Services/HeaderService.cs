using Dashboard.ViewModels;

namespace Dashboard.Services
{
    public class HeaderService : IHeaderService
    {
        public HeaderViewModel GetHeaderData()
        {
            // Exemple : données statiques ou temporaires
            return new HeaderViewModel
            {
                Plant = "Plant A",
                Project = "Project X",
                Family = "Family 1",
                ControlNum = "CN-00123"
            };
        }
    }
}
