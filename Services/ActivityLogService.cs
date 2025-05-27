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
        public async Task<List<ActivityLog>> GetLogsAsync()
        {
            return await _context.ActivityLogs
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();
        }

    }
}
