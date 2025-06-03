using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            var role = User.IsInRole("Admin") ? "Admin" :
                       User.IsInRole("TeamLeader") ? "Teamleader" : "Unknown";

            ViewBag.Role = role;
            ViewBag.UserName = User.Identity.Name;

            return View();
        }
    }
}