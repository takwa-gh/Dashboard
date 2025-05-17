using Dashboard.Data;
using Dashboard.Models;
using Dashboard.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
        //recuperation des stations selon le role de l'user
        public async Task<List<StationViewModel>> GetStationsForManagerAsync(int userId)
        {
            var stations = await _context.Stations
                .Where(s => s.UserId == userId)
                .ToListAsync();

            return stations.Select(station => new StationViewModel
            {
                StationId = station.StationId,
                StationName = station.StationName,
                GumValue = station.GumValue,
                AwtValue = station.AwtValue,
                PartNumber = station.PartNumber,
                DirectOperator = station.DirectOperator,
                IndirectOperator = station.IndirectOperator
            }).ToList();
        }

        public async Task<List<StationViewModel>> GetStationsForAdminAsync(string? userName)
        {
            var query = _context.Stations
                .Include(s => s.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(userName))
            {
                var lowered = userName.ToLower();
                query = query.Where(s => s.User != null && s.User.UserName.ToLower().Contains(lowered));
            }

            var stations = await query.OrderBy(s => s.StationId).ToListAsync();

            return stations.Select(station => new StationViewModel
            {
                StationId = station.StationId,
                StationName = station.StationName,
                GumValue = station.GumValue,
                AwtValue = station.AwtValue,
                PartNumber = station.PartNumber,
                DirectOperator = station.DirectOperator,
                IndirectOperator = station.IndirectOperator,
               
            }).ToList();
        }
      
        // create new station dans bd
        public async Task<bool> CreateStationAsync(CreateStationViewModel model, int userId)
        {
            var station = new Station()
            {
                StationName = model.StationName,
                GumValue = model.GumValue,
                AwtValue = model.AwtValue,
                PartNumber = model.PartNumber,
                DirectOperator = model.DirectOperator,
                IndirectOperator = model.IndirectOperator,
                UserId = userId,
                
            };

            _context.Stations.Add(station);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> EditStationAsync(EditStationViewModel model, int userId)
        {
            var station = await _context.Stations.FindAsync(model.StationId);
            if (station == null) return false;

            // Seul le manager propriétaire peut modifier
            if (station.UserId != userId)
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
        public async Task<bool> DeleteStationAsync(int id, int userId)
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
