using Dashboard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Controllers
{
    public class ActivityLogController : Controller
    {
        private readonly IActivityLogService _activityLogService;

        public ActivityLogController(IActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string userName, DateTime? startDate, DateTime? endDate)
        {
            var logs = await _activityLogService.GetFilteredLogsAsync(userName, startDate, endDate);
            return View(logs);
        }

    }
}
