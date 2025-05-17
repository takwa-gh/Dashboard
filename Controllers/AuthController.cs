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
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        //action declanche en envoyant les informations du connexion
        public async Task<IActionResult> Login(LoginViewModel model)

        {
            //validation des champs du formulaire
            if (!ModelState.IsValid) 
                return View(model); // return de meme view + message d'erreur
            //appel de service de login
            var isLoginSuccessful = await _authService.Login(model, HttpContext);
            //si le login est reussi, redirection vers la page d'accueil de dashboard
            if (isLoginSuccessful)
                return RedirectToAction("Index", "Dashboard");
            //sinon une erreur de connexion
            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _authService.Logout(HttpContext);
            return RedirectToAction("Login");
        }

        public IActionResult SignUp() => View();

        [HttpPost]
        //action declanche en envoyant les informations du formulaire d'inscription
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            // validation des touts le champs de formulaire d'inscription
            if (!ModelState.IsValid) 
                return View(model);
            if(model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                return View(model);
            }

            var isSignedUp = await _authService.SignUp(model);
            if (isSignedUp)
                return RedirectToAction("Login");

            ModelState.AddModelError("", "Email already exists.");
            return View(model);
        }
       
        
        
        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize] // necessite de connexion pour changer pwd
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            //verifier les champs sont valides 
            if (!ModelState.IsValid)
                return View(model);

            //recuperer l'ID de l'utilisateur connecté

            if (!int.TryParse(User.FindFirst("UserId")?.Value, out var userId))
                return Unauthorized();

            //appel de service de changement de mot de passe
            var result = await _authService.ChangePassword(userId, model.CurrentPassword, model.NewPassword);

            if (!result)
            {
                ModelState.AddModelError("CurrentPassword", "Current password is incorrect.");
                return View(model);
            }

            ViewBag.Message = "password updated successfully.";
            return View();
        }
    }
}





