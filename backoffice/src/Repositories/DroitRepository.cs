using Droits.Data;
using Droits.Exceptions;
using Droits.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Droits.Repositories;

public interface IDroitRepository
{
    Task<List<Droit>> GetDroitsAsync();
    Task<Droit> GetDroitAsync(Guid id);
    Task<Droit> AddDroitAsync(Droit droit);
    Task<Droit> UpdateDroitAsync(Droit droit);
    Task<WreckMaterial> UpdateWreckMaterialAsync(WreckMaterial wreckMaterial);
    Task<WreckMaterial> GetWreckMaterialAsync(Guid id,Guid droitId);
    Task<WreckMaterial> AddWreckMaterialAsync(WreckMaterial wreckMaterial);
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
        var droit = await _context.Droits.Include(d => d.Wreck).Include(d => d.WreckMaterials).FirstOrDefaultAsync(d => d.Id == id);
        if (droit == null)
        {
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

    public async Task<WreckMaterial> UpdateWreckMaterialAsync(WreckMaterial wreckMaterial)
    {
        wreckMaterial.LastModified = DateTime.UtcNow;

        var savedWreckMaterial = _context.WreckMaterials.Update(wreckMaterial).Entity;
        await _context.SaveChangesAsync();

        return savedWreckMaterial;
    }


    public async Task<WreckMaterial> GetWreckMaterialAsync(Guid id, Guid droitId)
    {
        var wreckMaterial = await _context.WreckMaterials.FirstOrDefaultAsync(wm => wm.Id == id && wm.DroitId == droitId);
        if (wreckMaterial == null)
        {
            throw new WreckMaterialNotFoundException();
        }
        return wreckMaterial;
    }

    public async Task<WreckMaterial> AddWreckMaterialAsync(WreckMaterial wreckMaterial)
    {
        wreckMaterial.Created = DateTime.UtcNow;
        wreckMaterial.LastModified = DateTime.UtcNow;

        var savedWreckMaterial = _context.WreckMaterials.Add(wreckMaterial).Entity;
        await _context.SaveChangesAsync();

        return savedWreckMaterial;
    }
}
