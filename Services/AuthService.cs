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

        public async Task<bool> Login(LoginViewModel model, HttpContext httpContext)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email); 
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password)) 
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

        public async Task Logout(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<bool> SignUp(SignUpViewModel model)
        {
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                return false;

            var user = new User
            {
                Email = model.Email,
                UserName = model.UserName,
                Role = "Admin",
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangePassword(int userId, string currentPassword, string newPassword)
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
