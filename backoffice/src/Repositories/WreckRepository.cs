using Droits.Data;
using Droits.Exceptions;
using Droits.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Droits.Repositories;

public interface IWreckRepository
{
    Task<List<Wreck>> GetWrecksAsync();
    Task<Wreck> GetWreckAsync(Guid id);
    Task<Wreck> AddWreckAsync(Wreck wreck);
    Task<Wreck> UpdateWreckAsync(Wreck wreck);
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
        var wreck = await _context.Wrecks
                            .Include(w => w.Droits)
                            .FirstOrDefaultAsync(w => w.Id == id);
        if ( wreck == null )
        {
            throw new WreckNotFoundException();
        }

        return wreck;
    }


    public async Task<Wreck> AddWreckAsync(Wreck wreck)
    {
        wreck.Created = DateTime.UtcNow;
        wreck.LastModified = DateTime.UtcNow;

        var savedWreck = _context.Wrecks.Add(wreck).Entity;
        await _context.SaveChangesAsync();

        return savedWreck;
    }


    public async Task<Wreck> UpdateWreckAsync(Wreck wreck)
    {
        wreck.LastModified = DateTime.UtcNow;

        var savedWreck = _context.Wrecks.Update(wreck).Entity;
        await _context.SaveChangesAsync();

        return savedWreck;
    }
}