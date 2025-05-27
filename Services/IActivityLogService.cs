using Dashboard.Models;

namespace Dashboard.Services
{
    public interface IActivityLogService
    {

        Task<List<ActivityLog>> GetFilteredLogsAsync(string userName, DateTime? startDate, DateTime? endDate);
        Task LogAsync(string userName, string action);

    }
}
