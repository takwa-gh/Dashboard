using Dashboard.Models;
using Dashboard.ViewModels;

namespace Dashboard.Services
{
    public interface IStationService
    {
        Task<List<StationViewModel>> GetStationsForManagerAsync(int userId);
        Task<List<StationViewModel>> GetStationsForAdminAsync(string? userName);

        Task<bool> CreateStationAsync(CreateStationViewModel model, int userId);
        Task<bool> EditStationAsync(EditStationViewModel model, int userId);
        Task<bool> DeleteStationAsync(int id, int userId);

        // méthodes pour gestion des entrées GUM / AWT
        Task AddAWTEntryAsync(int stationId, double awtValue);
        Task AddGUMEntryAsync(int stationId, double gumValue);
    }


}
