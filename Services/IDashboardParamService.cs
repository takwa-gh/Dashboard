using Dashboard.ViewModels;

namespace Dashboard.Services
{
    public interface IDashboardParamService
    {
        Task<DashboardParamViewModel?> GetDashboardParamsAsync();

        Task UpdateDashboardHeaderAsync(DashboardHeaderViewModel model);  // Pour Admin
        Task UpdateDashboardInfoAsync(DashboardInfoViewModel model);    // Pour Manager
    }


}
