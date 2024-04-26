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
    Task<Salvor> AddAsync(Salvor salvor, bool updateLastModified = true);
    Task<Salvor> UpdateAsync(Salvor salvor, bool updateLastModified = true);
    Task<Salvor?> GetSalvorByEmailAddressWithAssociationsAsync(string? salvorInfoEmail);
    Task<Salvor?> GetSalvorByEmailAddressAsync(string? salvorInfoEmail);
    Task<Salvor> GetSalvorByPowerappsIdAsync(string powerappsId);
    Task<Salvor?> GetSalvorByNameAndAddressAsync(string? name, Address? address);

}

public class SalvorRepository : BaseEntityRepository<Salvor>, ISalvorRepository
{
    public SalvorRepository(DroitsContext dbContext, IAccountService accountService) : base(dbContext,accountService)
    {
    }


    public IQueryable<Salvor> GetSalvors()
    {
        return Context.Salvors
            .OrderByDescending(l => l.Created);
    }


    public IQueryable<Salvor> GetSalvorsWithAssociations()
    {
        return Context.Salvors
            .Include(s => s.Droits)
            .OrderByDescending(l => l.Created)
            .AsNoTracking();
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
    public async Task<Salvor?> GetSalvorByEmailAddressWithAssociationsAsync(string? emailAddress) => await Context.Salvors
        .Include(s => s.Droits).ThenInclude(d => d.WreckMaterials)
        .FirstOrDefaultAsync(s => s.Email.Trim().ToLower().Equals(emailAddress));
    
    
    public async Task<Salvor> GetSalvorByPowerappsIdAsync(string powerappsId)
    {
        var salvor = await Context.Salvors
            .FirstOrDefaultAsync(s => s.PowerappsContactId == powerappsId);
        if ( salvor == null )
        {
            throw new SalvorNotFoundException();
        }

        return salvor;
    }
    
    
    public async Task<Salvor?> GetSalvorByNameAndAddressAsync(string? name, Address? address)
    {
        if (string.IsNullOrEmpty(name) || address == null || string.IsNullOrWhiteSpace(address.Line1))
        {
            return null;
        }

        var foundSalvor = await Context.Salvors
            .FirstOrDefaultAsync(s =>
                string.Equals(s.Name.Trim().ToLower(), name.Trim().ToLower()) &&
                !string.IsNullOrWhiteSpace(s.Address.Line1) &&
               s.Address.Line1.Equals(s.Address.Line1)
                &&
                !string.IsNullOrWhiteSpace(s.Address.Postcode) &&
                s.Address.Postcode.Equals(s.Address.Postcode)
            );

        return foundSalvor;
    }


}