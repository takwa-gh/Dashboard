using Dashboard.Data;
using Dashboard.Models;
using Dashboard.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace Dashboard.Services
{
    public class LineParamsService : ILineParamsService
    {
        private readonly AppDbContext _context;

        public LineParamsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LineParamViewModel> GetDashboardParamsAsync()
        {
            var param = await _context.LineParams.FirstOrDefaultAsync();
            if (param == null) return new LineParamViewModel
            {
                DashboardHeader = new DashboardHeaderViewModel(),
                DashboardInfo = new DashboardInfoViewModel()
            };
                
            return new LineParamViewModel
            {
                DashboardHeader = new DashboardHeaderViewModel
                {
                    Plant = param.Plant,
                    Project = param.Project,
                    Family = param.Family,
                    ControlNumber = param.ControlNumber
                },
                DashboardInfo = new DashboardInfoViewModel
                {
                    TactTime = param.TactTime,
                    ConveyorSpeed = param.ConveyorSpeed,
                    TargetQuantity = param.TargetQuantity,
                    WorkingTime = param.WorkingTime,
                    ActualOutput = param.ActualOutput,
                    CycleTime = param.CycleTime
                },
            };
        }
        public async Task<DashboardHeaderViewModel> GetDashboardHeaderAsync()
        {
            var param = await _context.LineParams.FirstOrDefaultAsync();
            if (param == null) return new DashboardHeaderViewModel();

            return new DashboardHeaderViewModel
            {
                Plant = param.Plant,
                Project = param.Project,
                Family = param.Family,
                ControlNumber = param.ControlNumber
            };
        }

        public async Task<DashboardInfoViewModel> GetDashboardInfoAsync()
        {
            var param = await _context.LineParams.FirstOrDefaultAsync();
            if (param == null) return new DashboardInfoViewModel();

            return new DashboardInfoViewModel
            {
                TactTime = param.TactTime,
                ConveyorSpeed = param.ConveyorSpeed,
                TargetQuantity = param.TargetQuantity,
                WorkingTime = param.WorkingTime,
                ActualOutput = param.ActualOutput,
                CycleTime = param.CycleTime
            };
        }

        public async Task UpdateDashboardInfoAsync(DashboardInfoViewModel model)
        {
            var param = await _context.LineParams.FirstOrDefaultAsync();

            if (param == null)
            {
                param = new LineParam
                {
                    ConveyorSpeed = model.ConveyorSpeed,
                    TactTime = model.TactTime,
                    ActualOutput = model.ActualOutput,
                    TargetQuantity = model.TargetQuantity,
                    WorkingTime = model.WorkingTime,
                    CycleTime = model.CycleTime
                };

                _context.LineParams.Add(param);
            }
            else
            {
                param.ConveyorSpeed = model.ConveyorSpeed;
                param.TactTime = model.TactTime;
                param.ActualOutput = model.ActualOutput;
                param.TargetQuantity = model.TargetQuantity;
                param.WorkingTime = model.WorkingTime;
                param.CycleTime = model.CycleTime;
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateDashboardHeaderAsync(DashboardHeaderViewModel model)
        {
            var entity = await _context.LineParams.FirstOrDefaultAsync();

            if (entity == null)
            {
                // Aucun enregistrement existant : on en crée un nouveau
                entity = new LineParam
                {
                    Plant = model.Plant,
                    Project = model.Project,
                    Family = model.Family,
                    ControlNumber = model.ControlNumber
                };

                _context.LineParams.Add(entity);
            }
            else
            {
                // Enregistrement existant : mise à jour
                entity.Plant = model.Plant;
                entity.Project = model.Project;
                entity.Family = model.Family;
                entity.ControlNumber = model.ControlNumber;
            }

            await _context.SaveChangesAsync();
        }


    }
}
    







