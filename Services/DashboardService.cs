using Dashboard.Data;
using Dashboard.Models;
using Dashboard.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }
        public DashboardHeaderViewModel GetDashboardHeader()
        {
            var latestParams = _context.DashboardParams
                .OrderByDescending(b => b.Id)
                .FirstOrDefault();
            if (latestParams == null)
            {
                return new DashboardHeaderViewModel
                {
                    Plant = string.Empty,
                    Project = string.Empty,
                    Family = string.Empty,
                    ControlNumber = string.Empty
                };
            }
            return new DashboardHeaderViewModel
            {
                Plant = latestParams.Plant,
                Project = latestParams.Project,
                Family = latestParams.Family,
                ControlNumber = latestParams.ControlNumber
            };
        }
        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            // 1. Charger toutes les stations
            var stations = await _context.Stations.ToListAsync();
            
            // 2. Charger les derniers paramètres globaux
            var latestParams = await _context.DashboardParams
                .OrderByDescending(b => b.Id)
                .FirstOrDefaultAsync();
            // Normalisation
            double tactTime = Normalize(latestParams?.TactTime ?? 0);
            double conveyorSpeed = Normalize(latestParams?.ConveyorSpeed ?? 0);
            double cycleTime = Normalize(latestParams?.CycleTime ?? 0);
            double actualOutput = Normalize(latestParams?.ActualOutput ?? 0);
            double workingTime = Normalize(latestParams?.WorkingTime ?? 0);
            double targetQuantity = Normalize(latestParams?.TargetQuantity ?? 0);
            string plant = latestParams?.Plant ?? string.Empty;
            string project = latestParams?.Project ?? string.Empty;
            string family = latestParams?.Family ?? string.Empty;
            string controlNumber = latestParams?.ControlNumber ?? string.Empty;

            // 3. Calculs des indicateurs
            var totalGum = stations.Sum(s => s.AverageGumValue); 
            var totalAwt = stations.Sum(s => s.AverageAwtValue);

            double totalManpower = Normalize(stations.Sum(s => s.DirectOperator + s.IndirectOperator));
            double manpowerNeeded = (tactTime > 0) ? Normalize(totalAwt / tactTime) : 1;
            double manpowerAllocation = (manpowerNeeded > 0) ? Math.Round(totalManpower / manpowerNeeded * 100, 2) : 0;
           
            double totalAbsDiff = stations.Sum(s => Math.Abs(cycleTime - conveyorSpeed));
            double lineEffectiveness = 0;
            if (totalManpower > 0 && conveyorSpeed > 0)
            {
                lineEffectiveness = Math.Round((totalAbsDiff / (totalManpower * conveyorSpeed)) * 100, 2);
            }
            // 4. Retourner le ViewModel complet
            var dashboardViewModel = new DashboardViewModel
            {
                Stations = stations,
                TotalGum = totalGum,
                TotalAwt = totalAwt,
                TotalManpower = totalManpower,
                ManpowerNeeded = manpowerNeeded,
                ManpowerAllocation = manpowerAllocation,
                LineEffectiveness = lineEffectiveness,
                DashboardParams = new DashboardParamViewModel
                {
                    DashboardInfo = new DashboardInfoViewModel
                    {
                        TactTime = tactTime,
                        ConveyorSpeed = conveyorSpeed,
                        CycleTime = cycleTime,
                        ActualOutput = actualOutput,
                        WorkingTime = workingTime,
                        TargetQuantity = targetQuantity },
                    DashboardHeader = new DashboardHeaderViewModel 
                    { 
                    Plant = plant,
                    Project = project,
                    Family = family,
                    ControlNumber = controlNumber }

                },
            };
            return dashboardViewModel;
        }
        private double Normalize(double value)
        {
            return double.IsNaN(value) || double.IsInfinity(value) ? 0 : value;
        }
    }
}