using APBD8.Context;
using APBD8.DTO;
using APBD8.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD8.Services;

public interface ITripService
{
    Task<IEnumerable<Trip>> GetAll();
    Task<IEnumerable<Trip>> GetByClient(int idClient);
    Task<IEnumerable<Trip>> GetByStartDate(DateTime dateTime);
}

public class TripService : ITripService
{
    private readonly MasterContext _context;

    public TripService(MasterContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Trip>> GetAll()
    {
        return await _context.Trips.ToListAsync();
    }

    public async Task<IEnumerable<Trip>> GetByClient(int idClient)
    {
        return await _context.Trips.Include(e => e.ClientTrips)
            .Where(e => e.ClientTrips.Any())
            .OrderByDescending(e => e.DateFrom)
            .ToListAsync();
    }

    public async Task<IEnumerable<Trip>> GetByStartDate(DateTime dateTime)
    {
        return await _context.Trips
            .Where(e => e.DateFrom == dateTime)
            .OrderByDescending(e => e.DateFrom)
            .ToListAsync();
    }
}