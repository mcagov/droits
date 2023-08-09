using Droits.Data;
using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Services;
using Microsoft.EntityFrameworkCore;

namespace Droits.Repositories;

public interface ISalvorRepository
{
    IQueryable<Salvor> GetSalvors();
    IQueryable<Salvor> GetSalvorsWithAssociations();
    Task<Salvor> GetSalvorAsync(Guid id);
    Task<Salvor> AddAsync(Salvor salvor);
    Task<Salvor> UpdateAsync(Salvor salvor);
}

public class SalvorRepository : BaseRepository<Salvor>, ISalvorRepository
{
    public SalvorRepository(DroitsContext dbContext, ICurrentUserService currentUserService) : base(dbContext,currentUserService)
    {
    }


    public IQueryable<Salvor> GetSalvors()
    {
        return Context.Salvors.OrderByDescending(l => l.LastModified);
    }


    public IQueryable<Salvor> GetSalvorsWithAssociations()
    {
        return GetSalvors();
    }


    public async Task<Salvor> GetSalvorAsync(Guid id)
    {
        var salvor = await Context.Salvors
            .Include(s => s.Droits)
            .Include(s => s.LastModifiedByUser)
            .FirstOrDefaultAsync(s => s.Id == id);
        if ( salvor == null )
        {
            throw new SalvorNotFoundException();
        }

        return salvor;
    }
}