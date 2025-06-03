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
                await _activityLogService.LogAsync(model.UserName, "Successful connection");

                
                return RedirectToAction("Index", "Home");
                
            }

            // Connexion échouée
            await _activityLogService.LogAsync(model.UserName, "Connexion Failed");

             TempData["ErrorMessage"] = "Invalid email or password.";
           
             return View(model);
         }

         public async Task<IActionResult> Logout()
         {
         
             await _authService.Logout(HttpContext);
             await _activityLogService.LogAsync(User.Identity?.Name, "Log out");
            // Logout

           
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
                 ViewBag.ErrorMessage = "Passwords do not match.";
                 return View(model);
             }

             var isSignedUp = await _authService.SignUp(model);

             if (isSignedUp)
             {
                
                TempData["successMessage"]= "Sign up successful .You can now log in.";


                return RedirectToAction("Login");
             }

            ModelState.AddModelError("Email", "Email already exists.");
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
                ModelState.AddModelError("CurrentPassword","Current password is incorrect");
                return View(model);
             }

            await _activityLogService.LogAsync(User.Identity?.Name, "Password change");


            TempData["SuccessMessage"] = "Password have been updated successfully.";
            return RedirectToAction("Index", "Home");
         }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(); 
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            var profile = await _authService.GetUserProfileAsync(userId);
            return View(profile);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserProfileViewModel model)
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            if (!ModelState.IsValid) return View("Profile", model);

            await _authService.UpdateUserProfileAsync(userId, model);
            return RedirectToAction("Index","Home");
        }

    }

 }