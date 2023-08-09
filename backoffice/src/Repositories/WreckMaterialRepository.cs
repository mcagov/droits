using System.Transactions;
using Droits.Data;
using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Services;
using Microsoft.EntityFrameworkCore;

namespace Droits.Repositories;

public interface IWreckMaterialRepository
{

    Task<WreckMaterial> AddAsync(WreckMaterial wreckMaterial);
    Task<WreckMaterial> UpdateAsync(WreckMaterial wreckMaterial);
    Task<WreckMaterial> GetWreckMaterialAsync(Guid id, Guid droitId);
    Task DeleteWreckMaterialForDroitAsync(Guid droitId, IEnumerable<Guid> wmToKeep);
}

public class WreckMaterialRepository : BaseRepository<WreckMaterial>, IWreckMaterialRepository
{
    public WreckMaterialRepository(DroitsContext dbContext, ICurrentUserService currentUserService) : base(dbContext,currentUserService)
    {

    }
    public async Task<WreckMaterial> GetWreckMaterialAsync(Guid id, Guid droitId)
    {
        var wreckMaterial =
            await Context.WreckMaterials.FirstOrDefaultAsync(wm =>
                wm.Id == id && wm.DroitId == droitId);
        if ( wreckMaterial == null )
        {
            throw new WreckMaterialNotFoundException();
        }

        return wreckMaterial;
    }
    public async Task DeleteWreckMaterialForDroitAsync(Guid droitId, IEnumerable<Guid> wmToKeep)
    {
        var wreckMaterials = await Context.WreckMaterials
            .Where(wm => wm.DroitId == droitId && !wmToKeep.Contains(wm.Id))
            .ToListAsync();

        Context.WreckMaterials.RemoveRange(wreckMaterials);
        await Context.SaveChangesAsync();
    }
    
}