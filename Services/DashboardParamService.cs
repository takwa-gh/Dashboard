using Dashboard.Data;
using Dashboard.Models;
using Dashboard.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace Dashboard.Services
{
    public class DashboardParamService : IDashboardParamService
    {
        private readonly AppDbContext _context;

        public DashboardParamService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardParamViewModel> GetDashboardParamsAsync()
        {
            var param = await _context.DashboardParams.FirstOrDefaultAsync();
            if (param == null) return new DashboardParamViewModel
            {
                DashboardHeader = new DashboardHeaderViewModel(),
                DashboardInfo = new DashboardInfoViewModel()
            };
                

            return new DashboardParamViewModel
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
            var param = await _context.DashboardParams.FirstOrDefaultAsync();
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
            var param = await _context.DashboardParams.FirstOrDefaultAsync();
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
            var param = _context.DashboardParams.FirstOrDefault();

            if (param != null)
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
            var entity = await _context.DashboardParams.FirstOrDefaultAsync();
            if (entity == null) return;

            entity.Plant = model.Plant;
            entity.Project = model.Project;
            entity.Family = model.Family;
            entity.ControlNumber = model.ControlNumber;

            await _context.SaveChangesAsync();
        }

    }
}
    







