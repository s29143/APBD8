using APBD8.Context;
using APBD8.Exceptions;
using APBD8.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD8.Services;

public interface IClientService
{
    Task<Client?> GetById(int id);
    Task<bool> Delete(int id);
}


public class ClientService : IClientService
{
    private readonly MasterContext _context;

    public ClientService(MasterContext context)
    {
        _context = context;
    }

    public async Task<Client?> GetById(int id)
    {
        return await _context.Clients.Where(e => e.IdClient == id).SingleOrDefaultAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var client = await GetById(id);
        if (client is null)
            throw new NotFoundException("");
        if (client.ClientTrips.Count() > 0)
        {
            return false;
        }
        return await _context.Clients.Where(e => e.IdClient == id).ExecuteDeleteAsync() > 0;
    }
}