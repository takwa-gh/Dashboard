using Dashboard.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Index()
        {
            var logs = await _activityLogService.GetLogsAsync();
            return View(logs);
        }

    }
}
