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
                if (user == null)
                {
                    
                    return false;
                }

                if (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                {
                  
                    return false;
                }

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("UserId", user.UserId.ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };

                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims, "Login")));
                return true;
            }
                return false;
            }
        }

        public async Task Logout(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<bool> SignUp(SignUpViewModel model)
        {
            try
            {
                _logger.LogInformation("Tentative d'inscription pour l'email: {Email}", model.Email);

                if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                {
                    _logger.LogWarning("Tentative d'inscription avec un email existant: {Email}", model.Email);
                    return false;
                }

                var user = new User
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    Role = "Admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Inscription réussie pour l'utilisateur: {UserId}", user.UserId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'inscription pour l'email: {Email}", model.Email);
                return false;
            }
        }

        public async Task<bool> ChangePassword(int userId, string currentPassword, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || !BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
               _logger.LogWarning("Changement de mot de passe échoué - mot de passe incorrect pour l'utilisateur: {UserId}", userId);
               return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _context.SaveChangesAsync();
           
            return true;
        }

    }

}
