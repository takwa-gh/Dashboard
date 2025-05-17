using Dashboard.ViewModels;

namespace Dashboard.Services
{
    public interface IAuthService
    {
        Task<bool> Login(LoginViewModel model, HttpContext httpContext);
        Task Logout(HttpContext httpContext);
        Task<bool> SignUp(SignUpViewModel model);
        Task<bool> ChangePassword(int userId, string currentPassword, string newPassword);

    }

}
