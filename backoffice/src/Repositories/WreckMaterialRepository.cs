
#region

using Droits.Data;
using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Services;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Droits.Repositories;

public interface IWreckMaterialRepository
{

    Task<WreckMaterial> AddAsync(WreckMaterial wreckMaterial, bool updateLastModified = true);
    Task<WreckMaterial> UpdateAsync(WreckMaterial wreckMaterial, bool updateLastModified = true);
    Task<WreckMaterial> GetWreckMaterialAsync(Guid id);
    Task DeleteWreckMaterialForDroitAsync(Guid droitId, IEnumerable<Guid> wmToKeep);
    Task<WreckMaterial> GetWreckMaterialByPowerappsIdAsync(string powerappsId);
}

public class  WreckMaterialRepository : BaseEntityRepository<WreckMaterial>, IWreckMaterialRepository
{
    public WreckMaterialRepository(DroitsContext dbContext, IAccountService accountService) : base(dbContext,accountService)
    {

    }
    public async Task<WreckMaterial> GetWreckMaterialAsync(Guid id)
    {
        var wreckMaterial =
            await Context.WreckMaterials.Include(wm => wm.Images).Include(wm => wm.Files).FirstOrDefaultAsync(wm =>
                wm.Id == id);
        if ( wreckMaterial == null )
        {
            throw new WreckMaterialNotFoundException();
        }

        return wreckMaterial;
    }
    public async Task DeleteWreckMaterialForDroitAsync(Guid droitId, IEnumerable<Guid> wmToKeep)
    {
        var wreckMaterialsToDelete = await GetWreckMaterialsToDeleteAsync(droitId, wmToKeep);
    
        await RemoveWreckMaterialsFromDatabaseAsync(wreckMaterialsToDelete);
    }

    private async Task<List<WreckMaterial>> GetWreckMaterialsToDeleteAsync(Guid droitId, IEnumerable<Guid> wmToKeep)
    {
        return await Context.WreckMaterials
            .Where(wm => wm.DroitId == droitId && !wmToKeep.Contains(wm.Id))
            .ToListAsync();
    }

    private async Task RemoveWreckMaterialsFromDatabaseAsync(IEnumerable<WreckMaterial> wreckMaterialsToDelete)
    {
        Context.WreckMaterials.RemoveRange(wreckMaterialsToDelete);
        await Context.SaveChangesAsync();
    }
    
    public async Task<WreckMaterial> GetWreckMaterialByPowerappsIdAsync(string powerappsId)
    {
        var wreckMaterial = await Context.WreckMaterials
            .FirstOrDefaultAsync(w => w.PowerappsWreckMaterialId == powerappsId);
        if ( wreckMaterial == null )
        {
            throw new WreckMaterialNotFoundException();
        }

        return wreckMaterial;
    }

    
}