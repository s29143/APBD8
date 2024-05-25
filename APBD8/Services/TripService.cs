using APBD8.Context;
using APBD8.DTO;
using APBD8.Exceptions;
using APBD8.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD8.Services;

public interface ITripService
{
    Task<IEnumerable<Trip>> GetAll();
    Task<bool> AddToTrip(int idTrip, ClientDTO clientDto);
    
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

    public async Task<bool> AddToTrip(int idTrip, ClientDTO clientDto)
    {
        var trip = await _context.Trips.Where(e => e.IdTrip == idTrip).SingleOrDefaultAsync();
        if (trip is null)
        {
            throw new NotFoundException("Trip not found given id=" + idTrip);
        }

        var client = await _context.Clients.Where(e => e.Pesel == clientDto.Pesel).SingleOrDefaultAsync();
        if (client is null)
        {
            client = new Client()
            {
                Email = clientDto.Email,
                Pesel = clientDto.Pesel,
                Telephone = clientDto.Telephone,
                FirstName = clientDto.FirstName,
                LastName = clientDto.LastName
            };
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
        }

        if (_context.ClientTrips.Count(e => e.IdTrip == idTrip && e.IdClient == client.IdClient) > 0)
            return false;

        await _context.ClientTrips.AddAsync(new ClientTrip()
        {
            IdClient = client.IdClient,
            IdTrip = idTrip,
            PaymentDate = clientDto.PaymentDate,
            RegisteredAt = DateTime.Now
        });
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Trip>> GetByStartDate(DateTime dateTime)
    {
        return await _context.Trips
            .Where(e => e.DateFrom == dateTime)
            .OrderByDescending(e => e.DateFrom)
            .ToListAsync();
    }
}