using Dashboard.Data;
using Dashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Services
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly AppDbContext _context;

        public ActivityLogService(AppDbContext context)
        {
            _context = context;
        }
        public async Task LogAsync(string userName, string action)
        {
            var log = new ActivityLog
            {
                UserName = userName,
                Action = action,
               
            };

            _context.ActivityLogs.Add(log);
            await _context.SaveChangesAsync();
        }


        public async Task<List<ActivityLog>> GetFilteredLogsAsync(string userName, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.ActivityLogs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(userName))
            {
                query = query.Where(log => log.UserName.Contains(userName));
            }

            if (startDate.HasValue)
            {
                query = query.Where(log => log.Timestamp >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(log => log.Timestamp <= endDate.Value.AddDays(1).AddTicks(-1));
            }

            return await query.OrderByDescending(log => log.Timestamp).ToListAsync();
        }
    }
}