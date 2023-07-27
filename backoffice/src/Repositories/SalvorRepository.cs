using Droits.Data;
using Droits.Exceptions;
using Droits.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Droits.Repositories;

public interface ISalvorRepository
{
    Task<List<Salvor>> GetSalvorsAsync();
    Task<Salvor> GetSalvorAsync(Guid id);
    Task<Salvor> AddSalvorAsync(Salvor salvor);
    Task<Salvor> UpdateSalvorAsync(Salvor salvor);
}

public class SalvorRepository : ISalvorRepository
{
    private readonly DroitsContext _context;


    public SalvorRepository(DroitsContext dbContext)
    {
        _context = dbContext;
    }


    public async Task<List<Salvor>> GetSalvorsAsync()
    {
        return await _context.Salvors.ToListAsync();
    }


    public async Task<Salvor> GetSalvorAsync(Guid id)
    {
        var salvor = await _context.Salvors.Include(s => s.Droits).FirstOrDefaultAsync(s => s.Id == id);
        if ( salvor == null )
        {
            throw new SalvorNotFoundException();
        }

        return salvor;
    }


    public async Task<Salvor> AddSalvorAsync(Salvor salvor)
    {
        salvor.Created = DateTime.UtcNow;
        salvor.LastModified = DateTime.UtcNow;

        var savedSalvor = _context.Salvors.Add(salvor).Entity;
        await _context.SaveChangesAsync();

        return savedSalvor;
    }


    public async Task<Salvor> UpdateSalvorAsync(Salvor salvor)
    {
        salvor.LastModified = DateTime.UtcNow;

        var savedSalvor = _context.Salvors.Update(salvor).Entity;
        await _context.SaveChangesAsync();

        return savedSalvor;
    }
}