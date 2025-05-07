using Dashboard.Models;
using Dashboard.ViewModels;

namespace Dashboard.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserViewModel>> GetAllAsync();
        Task<UserViewModel?> GetByIdAsync(Guid id);
        Task CreateAsync(CreateUserViewModel model);
        Task<EditUserViewModel?> GetEditByIdAsync(Guid id);
        Task UpdateAsync(EditUserViewModel model);
        Task DeleteAsync(Guid id);
    }



}
