using Dashboard.ViewModels;

namespace Dashboard.Services
{
    public interface IKpiEvaluationService
    {
        public List<KpiAlertViewModel> EvaluateKpis(DashboardViewModel dashboard);
    }
}
