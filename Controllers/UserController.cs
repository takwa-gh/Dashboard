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
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
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
            if (!ModelState.IsValid) return View(model);

            await _userService.CreateAsync(model);
            return RedirectToAction("Users");
        }

        // Formulaire d’édition
        public async Task<IActionResult> Edit(Guid id)
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
            return RedirectToAction(nameof(Users));
        }

        // Confirmation de suppression
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // Traitement de suppression
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _userService.DeleteAsync(id);
            return RedirectToAction("Users");
        }
    }
}