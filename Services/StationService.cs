using Dashboard.Data;
using Dashboard.Models;
using Dashboard.Services;
using Dashboard.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class StationService : IStationService
{
    private readonly AppDbContext _context;

    public StationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<StationViewModel>> GetStationsForManagerAsync(int userId)
    {
        var stations = await _context.Stations
            .Include(s => s.User)
            .Include(s => s.GUMEntries)
            .Include(s => s.AWTEntries)
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
            IndirectOperator = station.IndirectOperator,
            UserId = station.UserId,
            User = station.User,

            GUMEntries = station.GUMEntries.Select(g => new StationGUM
            {
                Id = g.Id,
                Value = g.Value,
                Timestamp = g.Timestamp
            }).ToList(),

            AWTEntries = station.AWTEntries.Select(a => new StationAWT
            {
                Id = a.Id,
                Value = a.Value,
                Timestamp = a.Timestamp
            }).ToList(),

            MinGumValue = station.GUMEntries.Any() ? station.GUMEntries.Min(g => g.Value) : 0,
            MaxGumValue = station.GUMEntries.Any() ? station.GUMEntries.Max(g => g.Value) : 0,
            AverageGumValue = station.GUMEntries.Any() ? station.GUMEntries.Average(g => g.Value) : 0,

            MinAwtValue = station.AWTEntries.Any() ? station.AWTEntries.Min(a => a.Value) : 0,
            MaxAwtValue = station.AWTEntries.Any() ? station.AWTEntries.Max(a => a.Value) : 0,
            AverageAwtValue = station.AWTEntries.Any() ? station.AWTEntries.Average(a => a.Value) : 0,
            

        }).ToList();
    }

    public async Task<List<StationViewModel>> GetStationsForAdminAsync(string? userName)
    {
        var query = _context.Stations
            .Include(s => s.User)
            .Include(s => s.GUMEntries)
            .Include(s => s.AWTEntries)
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
            UserId = station.UserId,
            User = station.User,

            MinGumValue = station.GUMEntries.Any() ? station.GUMEntries.Min(g => g.Value) : 0,
            MaxGumValue = station.GUMEntries.Any() ? station.GUMEntries.Max(g => g.Value) : 0,
            AverageGumValue = station.GUMEntries.Any() ? station.GUMEntries.Average(g => g.Value) : 0,

            MinAwtValue = station.AWTEntries.Any() ? station.AWTEntries.Min(a => a.Value) : 0,
            MaxAwtValue = station.AWTEntries.Any() ? station.AWTEntries.Max(a => a.Value) : 0,
            AverageAwtValue = station.AWTEntries.Any() ? station.AWTEntries.Average(a => a.Value) : 0

        }).ToList();
    }

    public async Task<bool> CreateStationAsync(CreateStationViewModel model, int userId)
    {
        var station = new Station
        {
            StationName = model.StationName,
            GumValue = model.GumValue,
            AwtValue = model.AwtValue,
            PartNumber = model.PartNumber,
            DirectOperator = model.DirectOperator,
            IndirectOperator = model.IndirectOperator,
            UserId = userId
        };

        _context.Stations.Add(station);
        await _context.SaveChangesAsync(); // ✅ station.Id maintenant disponible

        if (model.GumValue > 0)
            await AddGUMEntryAsync(station.StationId, model.GumValue);

        if (model.AwtValue > 0)
            await AddAWTEntryAsync(station.StationId, model.AwtValue);

        return true;
    }

    public async Task<bool> EditStationAsync(EditStationViewModel model, int userId)
    {
        var station = await _context.Stations.FindAsync(model.StationId);
        if (station == null || station.UserId != userId)
            return false;

        station.StationName = model.StationName;
        station.GumValue = model.NewGumValue;
        station.AwtValue = model.NewAwtValue;
        station.PartNumber = model.PartNumber;
        station.DirectOperator = model.DirectOperator;
        station.IndirectOperator = model.IndirectOperator;

        await _context.SaveChangesAsync();

        if (model.NewGumValue > 0)
            await AddGUMEntryAsync(station.StationId, model.NewGumValue);

        if (model.NewAwtValue > 0)
            await AddAWTEntryAsync(station.StationId, model.NewAwtValue);

        return true;
    }

    public async Task<bool> DeleteStationAsync(int id, int userId)
    {
        var station = await _context.Stations.FirstOrDefaultAsync(s => s.StationId == id);
        if (station == null || station.UserId != userId)
            return false;

        _context.Stations.Remove(station);
        await _context.SaveChangesAsync();
        return true;
    }
    [HttpGet]
    public async Task AddAWTEntryAsync(int stationId, double awtValue)
    {
        var station = await _context.Stations
            .Include(s => s.AWTEntries)
            .FirstOrDefaultAsync(s => s.StationId == stationId);

        if (station == null) return;

        station.AWTEntries.Add(new StationAWT
        {
            Value = awtValue,
            StationId = stationId
        });

        await UpdateAwtStatsAsync(station);
        await _context.SaveChangesAsync();
    }
    [HttpPost]
    public async Task AddGUMEntryAsync(int stationId, double gumValue)
    {
        var station = await _context.Stations
            .Include(s => s.GUMEntries)
            .FirstOrDefaultAsync(s => s.StationId == stationId);

        if (station == null) return;

        station.GUMEntries.Add(new StationGUM
        {
            Value = gumValue,
            StationId = stationId
        });

        await UpdateGumStatsAsync(station);
        await _context.SaveChangesAsync();
    }

    private Task UpdateAwtStatsAsync(Station station)
    {
        var values = station.AWTEntries.Select(e => e.Value).ToList();
        station.MinAwtValue = values.Min();
        station.MaxAwtValue = values.Max();
        station.AverageAwtValue = values.Average();
        return Task.CompletedTask;
    }

    private Task UpdateGumStatsAsync(Station station)
    {
        var values = station.GUMEntries.Select(e => e.Value).ToList();
        station.MinGumValue = values.Min();
        station.MaxGumValue = values.Max();
        station.AverageGumValue = values.Average();
        return Task.CompletedTask;
    }
    public async Task DeleteGumEntryAsync(int id)
    {
        var entry = await _context.StationGUMs.FindAsync(id);
        if (entry != null)
        {
            _context.StationGUMs.Remove(entry);
            await _context.SaveChangesAsync();
        }
    }

    public async Task  DeleteAwtEntryAsync(int id)
    {
        var entry = await _context.StationAWTs.FindAsync(id);
        if (entry != null)
        {
            _context.StationAWTs.Remove(entry);
            await _context.SaveChangesAsync();
        }
    }

}

