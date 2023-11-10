#region

using Droits.Data;
using Droits.Exceptions;
using Droits.Helpers;
using Droits.Models.DTOs;
using Droits.Models.Entities;
using Droits.Services;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Droits.Repositories;

public interface IDroitRepository
{
    IQueryable<Droit> GetDroits();
    IQueryable<Droit> GetDroitsWithAssociations();
    Task<Droit> GetDroitWithAssociationsAsync(Guid id);
    Task<Droit> GetDroitAsync(Guid id);
    Task<Droit> AddAsync(Droit droit);
    Task<Droit> UpdateAsync(Droit droit);

    Task<bool> IsReferenceUnique(Droit droit);

    Task<int> GetYearDroitCount();
    
    Task<List<DroitExportDto>> SearchDroitsAsync(string query);
}

public class DroitRepository : BaseEntityRepository<Droit>, IDroitRepository
{
    public DroitRepository(DroitsContext dbContext, IAccountService accountService) : base(dbContext,accountService)
    {

    }


    public IQueryable<Droit> GetDroits()
    {
        return Context.Droits.Include(d => d.AssignedToUser).OrderByDescending(d => d.ReportedDate);
    }


    public IQueryable<Droit> GetDroitsWithAssociations()
    {
        return GetDroits()
            .Include(d => d.AssignedToUser)
            .Include(d => d.Letters)
            .Include(d => d.Wreck)
            .Include(d => d.Salvor)
            .Include(d => d.WreckMaterials);
    }


    public async Task<Droit> GetDroitWithAssociationsAsync(Guid id)
    {
        var droit = await Context.Droits
            .Include(d => d.AssignedToUser)
            .Include(d => d.Letters)

            .Include(d => d.Wreck).ThenInclude(w => w!.Notes).ThenInclude(n => n.LastModifiedByUser)
            .Include(d => d.Salvor).ThenInclude(s => s!.Notes).ThenInclude(n => n.LastModifiedByUser)
            .Include(d => d.WreckMaterials).ThenInclude(wm => wm.Images)

            .Include(d => d.Notes).ThenInclude(n => n.LastModifiedByUser)
            .Include(d => d.LastModifiedByUser)
            .FirstOrDefaultAsync(d => d.Id == id);
        if ( droit == null )
        {
            throw new DroitNotFoundException();
        }

        return droit;
    }


    public async Task<Droit> GetDroitAsync(Guid id)
    {
        var droit = await Context.Droits
                                .Include(d => d.WreckMaterials)
                                .Include(d => d.LastModifiedByUser)
                                .Include(d => d.AssignedToUser)
                                .FirstOrDefaultAsync(d => d.Id == id);
        if ( droit == null )
        {
            throw new DroitNotFoundException();
        }

        return droit;
    }


    public async Task<int> GetYearDroitCount()
    {
        return await Context.Droits.CountAsync(d => d.Created.Year == DateTime.UtcNow.Year);
    }
    public async Task<bool> IsReferenceUnique(Droit droit)
    {

        var foundDroits = await Context.Droits.Where(d => d.Reference == droit.Reference).ToListAsync();

        if ( !foundDroits.Any() )
        {
            return true;
        }
        
        if ( foundDroits.Count() > 1 )
        {
            return false;
        }

        var foundDroit = foundDroits.FirstOrDefault();
        
        return foundDroit == null || foundDroit.Id == droit.Id;
    }


    public async Task<List<DroitExportDto>> SearchDroitsAsync(string query)
    {
        
        if (string.IsNullOrWhiteSpace(query))
        {
            return new List<DroitExportDto>();
        }

        query = query.Trim().ToLower();

        var droits = await Context.Droits
            .Include(d => d.Wreck)
            .Include(d => d.Salvor)
            .Include(d => d.AssignedToUser)
            .Where(d =>
                SearchHelper.Matches(query, d.Reference) ||
                (d.Wreck != null && SearchHelper.Matches(query, d.Wreck.Name)) ||
                (d.Salvor != null && SearchHelper.Matches(query, d.Salvor.Name)) ||
                (d.AssignedToUser != null && SearchHelper.Matches(query, d.AssignedToUser.Name))
            )
            .ToListAsync();

        var results = droits.Select(d => new DroitExportDto(d)).ToList();

        return results;
    }
}