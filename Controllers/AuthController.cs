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
         private readonly IActivityLogService _activityLogService;

         public AuthController(IAuthService authService, IActivityLogService activityLogService)
         {
             _authService = authService;
             _activityLogService = activityLogService;
         }

         [HttpGet]
         public IActionResult Login() => View();

         [HttpPost]
         public async Task<IActionResult> Login(LoginViewModel model)
         {
             if (!ModelState.IsValid)
                 return View(model);

             var isLoginSuccessful = await _authService.Login(model, HttpContext);

             if (isLoginSuccessful)
             {
                await _activityLogService.LogAsync(model.Email, "Successful connexion");

                return RedirectToAction("Index", "Dashboard");
             }

            // Connexion échouée
            await _activityLogService.LogAsync(model.Email, "Connexion Failed");

            ModelState.AddModelError("", "Invalid email or password.");
             return View(model);
         }

         public async Task<IActionResult> Logout()
         {
         
             await _authService.Logout(HttpContext);
             await _activityLogService.LogAsync(User.Identity?.Name, "Log out");
            
            return RedirectToAction("Login");
         }

         public IActionResult SignUp() => View();

         [HttpPost]
         public async Task<IActionResult> SignUp(SignUpViewModel model)
         {
             if (!ModelState.IsValid)
                 return View(model);

             if (model.Password != model.ConfirmPassword)
             {
                 ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                 return View(model);
             }

             var isSignedUp = await _authService.SignUp(model);

             if (isSignedUp)
             {
                await _activityLogService.LogAsync(User.Identity?.Name, "Successful SignUp");


                return RedirectToAction("Login");
             }

             ModelState.AddModelError("", "Email already exists.");
             return View(model);
         }

         [HttpGet]
         [Authorize]
         public IActionResult ChangePassword() => View();

         [HttpPost]
         [Authorize]
         public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
         {
             if (!ModelState.IsValid)
                 return View(model);

             if (!int.TryParse(User.FindFirst("UserId")?.Value, out var userId))
                 return Unauthorized();

             var result = await _authService.ChangePassword(userId, model.CurrentPassword, model.NewPassword);

             if (!result)
             {
                 ModelState.AddModelError("CurrentPassword", "Current password is incorrect.");
                 return View(model);
             }

            await _activityLogService.LogAsync(User.Identity?.Name, "Password change");


            ViewBag.Message = "Password updated successfully.";
             return View();
         }
     }

 }