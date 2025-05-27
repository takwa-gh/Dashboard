using Dashboard.Models;

namespace Dashboard.Services
{
    public interface IActivityLogService
    {
        Task LogAsync(string UserName, string Action);
        Task<List<ActivityLog>> GetLogsAsync();
    }
}
