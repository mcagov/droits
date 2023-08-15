using Droits.Data;
using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Services;
using Microsoft.EntityFrameworkCore;

namespace Droits.Repositories;

public interface IWreckRepository
{
    IQueryable<Wreck> GetWrecks();
    IQueryable<Wreck> GetWrecksWithAssociations();
    Task<Wreck> GetWreckAsync(Guid id);
    Task<Wreck> AddAsync(Wreck wreck);
    Task<Wreck> UpdateAsync(Wreck wreck);
}

public class WreckRepository : BaseEntityRepository<Wreck>, IWreckRepository
{
    public WreckRepository(DroitsContext dbContext, ICurrentUserService currentUserService) : base(dbContext,currentUserService)
    {
    }


    public IQueryable<Wreck> GetWrecks()
    {
        return Context.Wrecks.OrderByDescending(l => l.LastModified);
    }


    public IQueryable<Wreck> GetWrecksWithAssociations()
    {
        return GetWrecks();
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
}