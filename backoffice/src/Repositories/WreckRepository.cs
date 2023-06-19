using Droits.Exceptions;
using Droits.Models;
using Microsoft.EntityFrameworkCore;

namespace Droits.Repositories;

public interface IWreckRepository
{
    Task<List<Wreck>> GetWrecksAsync();
    Task<Wreck> GetWreckAsync(Guid id);
    Task<Wreck> AddWreckAsync(Wreck droit);
    Task<Wreck> UpdateWreckAsync(Wreck droit);
}

public class WreckRepository : IWreckRepository
{
    private readonly DroitsContext _context;

    public WreckRepository(DroitsContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<List<Wreck>> GetWrecksAsync()
    {
        return await _context.Wrecks.ToListAsync();
    }

    public async Task<Wreck> GetWreckAsync(Guid id)
    {
        var droit = await _context.Wrecks.FindAsync(id);
        if(droit == null){
            throw new WreckNotFoundException();
        }
        return droit;
    }


   public async Task<Wreck> AddWreckAsync(Wreck droit)
    {
        droit.Created = DateTime.UtcNow;
        droit.LastModified = DateTime.UtcNow;

        var savedWreck = _context.Wrecks.Add(droit).Entity;
        await _context.SaveChangesAsync();

        return savedWreck;
    }

    public async Task<Wreck> UpdateWreckAsync(Wreck droit)
    {
        droit.LastModified = DateTime.UtcNow;

        var savedWreck = _context.Wrecks.Update(droit).Entity;
        await _context.SaveChangesAsync();

        return savedWreck;
    }
}
