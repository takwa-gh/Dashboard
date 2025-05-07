using Dashboard.ViewModels;

namespace Dashboard.Services
{
    public interface IStationService
    {
        Task<IEnumerable<StationViewModel>> GetStationsForUserAsync(Guid userId, string role, string? userNameFilter = null);
        Task<EditStationViewModel?> GetEditStationAsync(Guid stationId);
        Task<bool> UpdateStationAsync(EditStationViewModel model, Guid currentUserId, string role);
        Task<CreateStationViewModel> InitCreateStationAsync(Guid userId);
        Task<bool> CreateStationAsync(CreateStationViewModel model, Guid userId, string role);
        Task<StationViewModel?> GetStationDetailsAsync(Guid id);
        Task<bool> DeleteStationAsync(Guid id, Guid userId);
    }

}
