using Dashboard.ViewModels;
using System.Reflection;

namespace Dashboard.Services
{
    public class KpiEvaluationService : IKpiEvaluationService
    {
        public List<KpiAlertViewModel> EvaluateKpis(DashboardViewModel dashboard)
        {
            var alerts = new List<KpiAlertViewModel>();

           
            if (dashboard.pourcentageAWTvsGUM <90)
            {
                alerts.Add(new KpiAlertViewModel
                {
                    Title = "AWT vs GUM <90%",
                    Message = "SWTD and best practices revision",
                    Severity = AlertLevel.Warning
                });
            }
            else if ((dashboard.pourcentageAWTvsGUM >=90) && (dashboard.pourcentageAWTvsGUM<100))
            {
                alerts.Add(new KpiAlertViewModel
                {
                    Title = "90 <= AWT vs Gum < 100",
                    Message = "Good. Continuous emprouvement",
                    Severity = AlertLevel.Info
                });

            }
            else
            {
                alerts.Add(new KpiAlertViewModel
                {
                    Title = "AWT vs Gum >= 100",
                    Message = "Improve process and find missing tasks",
                    Severity = AlertLevel.Warning
                });


            }



            if (dashboard.ManpowerAllocation < 80)
            {
                alerts.Add(new KpiAlertViewModel
                {
                    Title = "Manpower Allocation <80%",
                    Message = "Revise line concept,increase operator's number but first check actual output ",
                    Severity = AlertLevel.Warning
                });
            }
            else if ((dashboard.ManpowerAllocation >= 80) && (dashboard.ManpowerAllocation< 110))
            {
                alerts.Add(new KpiAlertViewModel
                {
                    Title = "80% <= Manpower Allocation < 110%",
                    Message = "Good. Continuous emprouvement",
                    Severity = AlertLevel.Info
                });

            }
            else
            {
                alerts.Add(new KpiAlertViewModel
                {
                    Title = "Manpower Allocation >= 100%",
                    Message = "Revise line concept , reduce operator's number but first check actual output ",
                    Severity = AlertLevel.Warning
                });


            }

            if (dashboard.LineEffectiveness < 15)
            {
                alerts.Add(new KpiAlertViewModel
                {
                    Title = "Line Effectiveness < 15%",
                    Message = "Good. Balanced line that need continuous emprouvement",
                    Severity = AlertLevel.Info
                });
            }
            else
            {
                alerts.Add(new KpiAlertViewModel
                {
                    Title = "AWT vs Gum >= 15",
                    Message = "Reduce GAP between CT and CS , Line concept revision needed to achieve `BALANCING` ",
                    Severity = AlertLevel.Warning
                });


            }

            return alerts;
        }
    }

}
