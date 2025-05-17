using Dashboard.ViewModels;

namespace Dashboard.Services
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardDataAsync();
        


    }
}
