using Dashboard.Data;
using Dashboard.Helpers;
using Dashboard.Models;
using Dashboard.Services;
using Dashboard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        // Injection de dépendance du service user
        private readonly IUserService _userService;
        private readonly IActivityLogService _activityLogService;
        
        public UserController(IUserService userService, IActivityLogService activityLogService)
        {
            _userService = userService;
            _activityLogService = activityLogService;
        }

        // Liste des utilisateurs
        public async Task<IActionResult> Users()
        {
            var users = await _userService.GetAllAsync();
            return View(users);
        }

        // Formulaire de création
        public IActionResult Create()
        {
            return View();
        }

        // Traitement de création
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (!ModelState.IsValid) return View("Create",model);

            var result = await _userService.CreateAsync(model);

            if (!result)
            {
                ModelState.AddModelError("Email", "A user with this email already exists.");
                return View("Create",model); // Retourne la vue avec l’erreur affichée
            }

            await _activityLogService.LogAsync(User.Identity?.Name, $"Create user : {model.UserName}");
            return RedirectToAction("Users");
        }

        // Formulaire d’édition
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _userService.GetEditByIdAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }

        // Traitement de l’édition
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _userService.UpdateAsync(model);
            await _activityLogService.LogAsync(User.Identity?.Name, $"Edit user : {model.UserName}");
            return RedirectToAction("Users");
        }

        // Confirmation de suppression
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return View("Delete",user);
        }

        // Traitement de suppression
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user != null)
            {
                await _userService.DeleteAsync(id);
                await _activityLogService.LogAsync(User.Identity?.Name, $"Suppression de l'utilisateur : {user.UserName}");
            }
            return RedirectToAction("Users");
        }
    }
}
