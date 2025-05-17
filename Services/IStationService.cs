using Dashboard.ViewModels;

namespace Dashboard.Services
{
    public interface IStationService
    {
        Task<List<StationViewModel>> GetStationsForManagerAsync(int userId);
        Task<List<StationViewModel>> GetStationsForAdminAsync(string? userName);
        
        Task<bool> EditStationAsync(EditStationViewModel model, int userId);
        Task<bool> CreateStationAsync(CreateStationViewModel model, int userId);
      
        Task<bool> DeleteStationAsync(int id, int userId);
    }

}
