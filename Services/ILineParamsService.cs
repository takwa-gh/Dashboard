﻿using Dashboard.ViewModels;

namespace Dashboard.Services
{
    public interface ILineParamsService
    {
        Task<LineParamViewModel?> GetDashboardParamsAsync();
        Task<DashboardHeaderViewModel> GetDashboardHeaderAsync();
        Task<DashboardInfoViewModel> GetDashboardInfoAsync();

        Task UpdateDashboardHeaderAsync(DashboardHeaderViewModel model);  // Pour Admin
        Task UpdateDashboardInfoAsync(DashboardInfoViewModel model);    // Pour Manager
    }


}
