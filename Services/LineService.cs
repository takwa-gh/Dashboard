using Dashboard.Data;
using Dashboard.Models;
using Dashboard.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Services
{
    public class LineService : ILineService
    {
        private readonly AppDbContext _context;

        public LineService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateLineAsync(CreateLineViewModel model)
        {
            var line = new Line
            {
                Name = model.Name,
                TactTime = model.TactTime,
                ConveyorSpeed = model.ConveyorSpeed,
                TargetQuantity = model.TargetQuantity,
                WorkingTime = model.WorkingTime,
                ActualOutput = model.ActualOutput,
                CycleTime = model.CycleTime
            };

            _context.Lines.Add(line);
            await _context.SaveChangesAsync();
        }

        public async Task<List<LineViewModel>> GetAllLinesAsync()
        {
            return await _context.Lines
                .Select(l => new LineViewModel
                {
                    Id = l.Id,
                    Name = l.Name
                })
                .ToListAsync();
        }
    }

}
