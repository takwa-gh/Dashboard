using Dashboard.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Dashboard.Data;
using Microsoft.EntityFrameworkCore;
using Dashboard.Models;

namespace Dashboard.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> LoginAsync(LoginViewModel model, HttpContext httpContext)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email); //Recherche de l'utilisateur par email
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password)) // Vérification du mot de passe
                return false;

            var claims = new List<Claim> // Création de liste des claims
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("UserId", user.UserId.ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var identity = new ClaimsIdentity(claims, "Login"); 
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return true;
        }

        public async Task LogoutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<bool> SignUpAsync(SignUpViewModel model)
        {
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                return false;

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = model.Email,
                UserName = model.UserName,
                Role = "Manager",
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UserProfileViewModel?> GetProfileAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return null;

            return new UserProfileViewModel
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role,

            };
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || !BCrypt.Net.BCrypt.Verify(currentPassword, user.Password))
                return false;

            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _context.SaveChangesAsync();
            return true;
        }

    }

}
