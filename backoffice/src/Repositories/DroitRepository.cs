using Droits.Models;
using Microsoft.EntityFrameworkCore;

namespace Droits.Repositories;

public interface IDroitRepository
{
    Task<List<Droit>> GetDroitsAsync();
    Task<Droit?> GetDroitAsync(Guid id);
    Task AddDroitAsync(Droit droit);
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

    public async Task<Droit?> GetDroitAsync(Guid id)
    {
        return await _context.Droits.FindAsync(id);
    }


    public async Task AddDroitAsync(Droit droit)
    {
        droit.Created = DateTime.UtcNow;
        droit.Modified = DateTime.UtcNow;

        _context.Droits.Add(droit);
        await _context.SaveChangesAsync();
    }
}