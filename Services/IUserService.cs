using Dashboard.Models;
using Dashboard.ViewModels;

namespace Dashboard.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserViewModel>> GetAllAsync();
        Task<UserViewModel?> GetByIdAsync(int id);
        Task CreateAsync(CreateUserViewModel model);
        Task<EditUserViewModel?> GetEditByIdAsync(int id);
        Task UpdateAsync(EditUserViewModel model);
        Task DeleteAsync(int id);
    }



}
