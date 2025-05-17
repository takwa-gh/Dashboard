using Dashboard.ViewModels;

namespace Dashboard.Services
{
    public interface IDashboardParamService
    {
        DashboardParamViewModel Get();
        void SaveOrUpdate(DashboardParamViewModel model);
    }


}
