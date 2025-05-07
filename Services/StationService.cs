using Dashboard.Data;
using Dashboard.Models;
using Dashboard.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Services
{
    public class StationService : IStationService
    {
        private readonly AppDbContext _context;

        public StationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StationViewModel>> GetStationsForUserAsync(Guid userId, string role, string? userNameFilter = null)
        {
            var query = _context.Stations
                .Include(s => s.User)
                .AsQueryable();

            if (role == "Manager")
            {
                query = query.Where(s => s.UserId == userId);
            }
            else if (role == "Admin" && !string.IsNullOrWhiteSpace(userNameFilter))
            {
                query = query.Where(s => s.User != null && s.User.UserName.Contains(userNameFilter));
            }

            var stations = await query.ToListAsync();

            return stations.Select(s => new StationViewModel
            {
                StationId = s.StationId,
                StationName = s.StationName,
                GumValue = s.GumValue,
                AwtValue = s.AwtValue,
                PartNumber = s.PartNumber,
                DirectOperator = s.DirectOperator,
                IndirectOperator = s.IndirectOperator,
                UserId = s.UserId,
                User = s.User!
            });
        }

        public async Task<CreateStationViewModel> InitCreateStationAsync(Guid userId)
        {
            return new CreateStationViewModel { UserId = userId };
        }

        public async Task<bool> CreateStationAsync(CreateStationViewModel model, Guid userId, string role)
        {
            if (role != "Manager" && role != "Admin")
                return false;

            var station = new Station
            {
                StationId = Guid.NewGuid(),
                StationName = model.StationName,
                GumValue = model.GumValue,
                AwtValue = model.AwtValue,
                PartNumber = model.PartNumber,
                DirectOperator = model.DirectOperator,
                IndirectOperator = model.IndirectOperator,
                UserId = userId
            };

            _context.Stations.Add(station);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<EditStationViewModel?> GetEditStationAsync(Guid stationId)
        {
            var s = await _context.Stations.FindAsync(stationId);
            if (s == null) return null;

            return new EditStationViewModel
            {
                StationId = s.StationId,
                StationName = s.StationName,
                GumValue = s.GumValue ?? 0,
                AwtValue = s.AwtValue ?? 0,
                PartNumber = s.PartNumber,
                DirectOperator = s.DirectOperator ?? 0,
                IndirectOperator = s.IndirectOperator ?? 0
            };
        }

        public async Task<bool> UpdateStationAsync(EditStationViewModel model, Guid currentUserId, string role)
        {
            var station = await _context.Stations.FindAsync(model.StationId);
            if (station == null) return false;

            // Seul le manager propriétaire peut modifier
            if (role == "Manager" && station.UserId != currentUserId)
                return false;

            station.StationName = model.StationName;
            station.GumValue = model.GumValue;
            station.AwtValue = model.AwtValue;
            station.PartNumber = model.PartNumber;
            station.DirectOperator = model.DirectOperator;
            station.IndirectOperator = model.IndirectOperator;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<StationViewModel?> GetStationDetailsAsync(Guid id)
        {
            var s = await _context.Stations.Include(st => st.User).FirstOrDefaultAsync(st => st.StationId == id);
            if (s == null) return null;

            return new StationViewModel
            {
                StationId = s.StationId,
                StationName = s.StationName,
                GumValue = s.GumValue,
                AwtValue = s.AwtValue,
               
                PartNumber = s.PartNumber,
                DirectOperator = s.DirectOperator,
                IndirectOperator = s.IndirectOperator,
                UserId = s.UserId,
                User = s.User!
            };
        }

        public async Task<bool> DeleteStationAsync(Guid id, Guid userId)
        {
            var station = await _context.Stations.FirstOrDefaultAsync(s => s.StationId == id);
            if (station == null) return false;

            // Un Manager peut seulement supprimer ses propres stations
            if (station.UserId != userId)
                return false;

            _context.Stations.Remove(station);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
