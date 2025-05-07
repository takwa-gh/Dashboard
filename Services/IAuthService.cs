using Dashboard.ViewModels;

namespace Dashboard.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginViewModel model, HttpContext httpContext);
        Task LogoutAsync(HttpContext httpContext);
        Task<bool> SignUpAsync(SignUpViewModel model);
        Task<UserProfileViewModel?> GetProfileAsync(Guid userId);
        Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);

    }

}
