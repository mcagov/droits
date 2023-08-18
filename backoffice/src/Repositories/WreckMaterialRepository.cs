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
    Task<WreckMaterial> GetWreckMaterialAsync(Guid id);
    Task DeleteWreckMaterialForDroitAsync(Guid droitId, IEnumerable<Guid> wmToKeep);
}

public class WreckMaterialRepository : BaseEntityRepository<WreckMaterial>, IWreckMaterialRepository
{
    public WreckMaterialRepository(DroitsContext dbContext, IAccountService accountService) : base(dbContext,accountService)
    {

    }
    public async Task<WreckMaterial> GetWreckMaterialAsync(Guid id)
    {
        var wreckMaterial =
            await Context.WreckMaterials.Include(wm => wm.Images).FirstOrDefaultAsync(wm =>
                wm.Id == id);
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