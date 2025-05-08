using Dashboard.ViewModels;

namespace Dashboard.Services
{
    public interface ILineService
    {
        Task CreateLineAsync(CreateLineViewModel model);
        Task<List<LineViewModel>> GetAllLinesAsync(); // Pour affichage liste
    }
}
