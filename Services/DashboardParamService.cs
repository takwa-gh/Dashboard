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

        public DashboardParamViewModel Get()
        {
            var param = _context.DashboardParams.FirstOrDefault();
            if (param == null) return new DashboardParamViewModel();

            return new DashboardParamViewModel
            {
                ConveyorSpeed = param.ConveyorSpeed,
                TactTime = param.TactTime,
                TargetQuantity = param.TargetQuantity,
                WorkingTime = param.WorkingTime,
                ActualOutput = param.ActualOutput,
                CycleTime = param.CycleTime

            };
        }

        public void SaveOrUpdate(DashboardParamViewModel model)
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
            else
            {
                _context.DashboardParams.Add(new DashboardParam
                {
                    ConveyorSpeed = model.ConveyorSpeed,
                    TactTime = model.TactTime,
                    ActualOutput = model.ActualOutput,
                    TargetQuantity = model.TargetQuantity,
                    WorkingTime = model.WorkingTime,
                    CycleTime = model.CycleTime

                });
            }

            _context.SaveChanges();
        }
    }

}





