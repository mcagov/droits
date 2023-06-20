using Droits.Exceptions;
using Droits.Models;
using Microsoft.EntityFrameworkCore;

namespace Droits.Repositories;

public interface IDroitRepository
{
    Task<List<Droit>> GetDroitsAsync();
    Task<Droit> GetDroitAsync(Guid id);
    Task<Droit> AddDroitAsync(Droit droit);
    Task<Droit> UpdateDroitAsync(Droit droit);
}

public class DroitRepository : IDroitRepository
{
    private readonly DroitsContext _context;

    public DroitRepository(DroitsContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<List<Droit>> GetDroitsAsync()
    {
        return await _context.Droits.ToListAsync();
    }

    public async Task<Droit> GetDroitAsync(Guid id)
    {
        var droit = await _context.Droits.FindAsync(id);
        if(droit == null){
            throw new DroitNotFoundException();
        }
        return droit;
    }


    public async Task<Droit> AddDroitAsync(Droit droit)
    {
        droit.Created = DateTime.UtcNow;
        droit.LastModified = DateTime.UtcNow;

        var savedDroit = _context.Droits.Add(droit).Entity;
        await _context.SaveChangesAsync();

        return savedDroit;
    }

    public async Task<Droit> UpdateDroitAsync(Droit droit)
    {
        droit.LastModified = DateTime.UtcNow;

        var savedDroit = _context.Droits.Update(droit).Entity;
        await _context.SaveChangesAsync();

        return savedDroit;
    }
}
