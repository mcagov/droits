#region

using Droits.Data;
using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Services;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Droits.Repositories;

public interface IWreckRepository
{
    IQueryable<Wreck> GetWrecks();
    IQueryable<Wreck> GetWrecksWithAssociations();
    Task<Wreck> GetWreckAsync(Guid id);
    Task<Wreck> GetWreckByPowerappsIdAsync(string powerappsId);
    Task<Wreck> AddAsync(Wreck wreck);
    Task<Wreck> UpdateAsync(Wreck wreck);
}

public class WreckRepository : BaseEntityRepository<Wreck>, IWreckRepository
{
    public WreckRepository(DroitsContext dbContext, IAccountService accountService) : base(dbContext,accountService)
    {
    }


    public IQueryable<Wreck> GetWrecks()
    {
        return Context.Wrecks.OrderByDescending(l => l.Created);
    }


    public IQueryable<Wreck> GetWrecksWithAssociations()
    {
        return GetWrecks().AsNoTracking();
    }


    public async Task<Wreck> GetWreckAsync(Guid id)
    {
        var wreck = await Context.Wrecks
            .Include(w => w.Droits)
            .Include(w => w.LastModifiedByUser)
            .Include(d => d.Notes).ThenInclude(n => n.LastModifiedByUser)
            .FirstOrDefaultAsync(w => w.Id == id);
        if ( wreck == null )
        {
            throw new WreckNotFoundException();
        }

        return wreck;
    }
    
    public async Task<Wreck> GetWreckByPowerappsIdAsync(string powerappsId)
    {
        var wreck = await Context.Wrecks
            .FirstOrDefaultAsync(w => w.PowerappsWreckId == powerappsId);
        if ( wreck == null )
        {
            throw new WreckNotFoundException();
        }

        return wreck;
    }
    
}