using Dashboard.ViewModels;

namespace Dashboard.Services
{
    public interface IDashboardService
    {
        public DashboardHeaderViewModel GetDashboardHeader();
        Task<DashboardViewModel> GetDashboardDataAsync();
        


    }
}
