using Dashboard.Services;
using Dashboard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dashboard.Controllers
{
    public class LineParamsController : Controller
    {
        private readonly ILineParamsService _service;
        private readonly IActivityLogService _activityLogService;

        public LineParamsController(ILineParamsService service, IActivityLogService activityLogService)
        {
            _service = service;
            _activityLogService = activityLogService;
        }

        [HttpGet]
        public async Task<IActionResult> EditParams()
        {
            var model = await _service.GetDashboardParamsAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditParams(LineParamViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Please fill in all required fields.";
                return View(model);
            }
            var userName = User.Identity?.Name;

            if (User.IsInRole("Admin"))
            {
                var oldValues = await _service.GetDashboardHeaderAsync();
                await _service.UpdateDashboardHeaderAsync(model.DashboardHeader);

                if (oldValues.Plant != model.DashboardHeader.Plant)
                    await _activityLogService.LogAsync(userName, $"Changed Plant: '{oldValues.Plant}' → '{model.DashboardHeader.Plant}'");

                if (oldValues.Project != model.DashboardHeader.Project)
                    await _activityLogService.LogAsync(userName, $"Changed Project: '{oldValues.Project}' → '{model.DashboardHeader.Project}'");

                if (oldValues.Family != model.DashboardHeader.Family)
                    await _activityLogService.LogAsync(userName, $"Changed Family: '{oldValues.Family}' → '{model.DashboardHeader.Family}'");

                if (oldValues.ControlNumber != model.DashboardHeader.ControlNumber)
                    await _activityLogService.LogAsync(userName, $"Changed Control Number: '{oldValues.ControlNumber}' → '{model.DashboardHeader.ControlNumber}'");
            }
            else if (User.IsInRole("TeamLeader"))
            {
                var oldValues = await _service.GetDashboardInfoAsync();
                await _service.UpdateDashboardInfoAsync(model.DashboardInfo);

                if (oldValues.TactTime != model.DashboardInfo.TactTime)
                    await _activityLogService.LogAsync(userName, $"Changed Tact Time: {oldValues.TactTime} → {model.DashboardInfo.TactTime}");

                if (oldValues.ConveyorSpeed != model.DashboardInfo.ConveyorSpeed)
                    await _activityLogService.LogAsync(userName, $"Changed Conveyor Speed: {oldValues.ConveyorSpeed} → {model.DashboardInfo.ConveyorSpeed}");

                if (oldValues.TargetQuantity != model.DashboardInfo.TargetQuantity)
                    await _activityLogService.LogAsync(userName, $"Changed Target Quantity: {oldValues.TargetQuantity} → {model.DashboardInfo.TargetQuantity}");

                if (oldValues.WorkingTime != model.DashboardInfo.WorkingTime)
                    await _activityLogService.LogAsync(userName, $"Changed Working Time: {oldValues.WorkingTime} → {model.DashboardInfo.WorkingTime}");

                if (oldValues.ActualOutput != model.DashboardInfo.ActualOutput)
                    await _activityLogService.LogAsync(userName, $"Changed Actual Output: {oldValues.ActualOutput} → {model.DashboardInfo.ActualOutput}");

                if (oldValues.CycleTime != model.DashboardInfo.CycleTime)
                    await _activityLogService.LogAsync(userName, $"Changed Cycle Time: {oldValues.CycleTime} → {model.DashboardInfo.CycleTime}");
            }

            TempData["Success"] = "Parametres updated successfully.";
            return RedirectToAction("Index", "Home");
        }
    }
            
}

