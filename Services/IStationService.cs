using Dashboard.ViewModels;

namespace Dashboard.Services
{
    public interface IStationService
    {
        Task<IEnumerable<StationViewModel>> GetStationsForUserAsync(string userId, string role, string? userNameFilter = null);
        Task<EditStationViewModel?> GetEditStationAsync(string stationId);
        Task<bool> UpdateStationAsync(EditStationViewModel model, string currentUserId, string role);
        Task<CreateStationViewModel> InitCreateStationAsync(string userId);
        Task<bool> CreateStationAsync(CreateStationViewModel model, string userId, string role);
        Task<StationViewModel?> GetStationDetailsAsync(string id);
        Task<bool> DeleteStationAsync(string id, string userId);
    }

}
