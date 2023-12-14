#region

using Droits.Data;
using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Services;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Droits.Repositories;

public interface ISalvorRepository
{
    IQueryable<Salvor> GetSalvors();
    IQueryable<Salvor> GetSalvorsWithAssociations();
    Task<Salvor> GetSalvorAsync(Guid id);
    Task<Salvor> AddAsync(Salvor salvor);
    Task<Salvor> UpdateAsync(Salvor salvor);
    Task<Salvor?> GetSalvorByEmailAddressAsync(string? salvorInfoEmail);
}

public class SalvorRepository : BaseEntityRepository<Salvor>, ISalvorRepository
{
    public SalvorRepository(DroitsContext dbContext, IAccountService accountService) : base(dbContext,accountService)
    {
    }


    public IQueryable<Salvor> GetSalvors()
    {
        return Context.Salvors.OrderByDescending(l => l.Created);
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
            .Include(d => d.Notes).ThenInclude(n => n.LastModifiedByUser)
            .FirstOrDefaultAsync(s => s.Id == id);
        if ( salvor == null )
        {
            throw new SalvorNotFoundException();
        }

        return salvor;
    }
    
    public async Task<Salvor?> GetSalvorByEmailAddressAsync(string? emailAddress) => await Context.Salvors
                .FirstOrDefaultAsync(s => s.Email.Trim().ToLower().Equals(emailAddress));
}