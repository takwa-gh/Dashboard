using Dashboard.Data;
using Dashboard.Helpers;
using Dashboard.Models;
using Dashboard.Services;
using Dashboard.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using System.IO.Compression;
using System.Security.Claims;

namespace Dashboard.Controllers
{   
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (await _authService.LoginAsync(model, HttpContext))
                return RedirectToAction("Index", "Dashboard");

            ModelState.AddModelError("", "Invalid login.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync(HttpContext);
            return RedirectToAction("Login");
        }
        
        public IActionResult SignUp() => View();

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (await _authService.SignUpAsync(model))
                return RedirectToAction("Login");

            ModelState.AddModelError("", "Email already in use.");
            return View(model);
        }
        public async Task<IActionResult> GetProfileAsync()
        {
            var userId = Guid.Parse(User.FindFirst("UserId")?.Value ?? "");
            var profile = await _authService.GetProfileAsync(userId);
            if (profile == null) return NotFound();
            return View(profile);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = Guid.Parse(User.FindFirst("UserId")?.Value ?? "");

            var result = await _authService.ChangePasswordAsync(userId, model.CurrentPassword, model.NewPassword);

            if (!result)
            {
                ModelState.AddModelError("", "Le mot de passe actuel est incorrect.");
                return View(model);
            }

            ViewBag.Message = "Mot de passe changé avec succès.";
            return View();
        }


    }

}



