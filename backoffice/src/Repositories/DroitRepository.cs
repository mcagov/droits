#region

using Droits.Data;
using Droits.Exceptions;
using Droits.Models.DTOs.Exports;
using Droits.Models.Entities;
using Droits.Models.FormModels;
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
    Task<Droit> GetDroitByPowerappsIdAsync(string powerappsId);
    Task<Droit> AddAsync(Droit droit, bool updateLastModified = true);
    Task<Droit> UpdateAsync(Droit droit, bool updateLastModified = true);

    Task<bool> IsReferenceUnique(Droit droit);

    Task<int> GetYearDroitCount();
    
    Task<List<DroitExportDto>> SearchDroitsAsync(string query);
    Task<Droit> GetDroitByReferenceAsync(string reference);

}

public class DroitRepository : BaseEntityRepository<Droit>, IDroitRepository
{
    public DroitRepository(DroitsContext dbContext, IAccountService accountService) : base(dbContext,accountService)
    {

    }


    public IQueryable<Droit> GetDroits()
    {
        return Context.Droits.Include(d => d.AssignedToUser).OrderByDescending(d => d.ReportedDate).AsNoTracking();
    }


    public IQueryable<Droit> GetDroitsWithAssociations()
    {
        return GetDroits()
            .Include(d => d.AssignedToUser).AsNoTracking()
            .Include(d => d.Letters).AsNoTracking()
            .Include(d => d.Wreck).AsNoTracking()
            .Include(d => d.Salvor).AsNoTracking()
            .Include(d => d.WreckMaterials.OrderBy(wm => wm.Created)).AsNoTracking();
    }


    public async Task<Droit> GetDroitWithAssociationsAsync(Guid id)
    {
        var droit = await Context.Droits
            .Include(d => d.AssignedToUser).AsNoTracking()
            .Include(d => d.Letters).AsNoTracking()

            .Include(d => d.Wreck).ThenInclude(w => w!.Notes).ThenInclude(n => n.LastModifiedByUser)
            .Include(d => d.Salvor).ThenInclude(s => s!.Notes).ThenInclude(n => n.LastModifiedByUser)
            .Include(d => d.WreckMaterials.OrderBy(wm => wm.Created)).ThenInclude(wm => wm.Images)
            .Include(d => d.WreckMaterials.OrderBy(wm => wm.Created)).ThenInclude(wm => wm.Files)
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
                                .Include(d => d.WreckMaterials.OrderBy(wm => wm.Created)).ThenInclude(wm => wm.Images)
                                .Include(d => d.LastModifiedByUser)
                                .Include(d => d.AssignedToUser)
                                .FirstOrDefaultAsync(d => d.Id == id);
        if ( droit == null )
        {
            throw new DroitNotFoundException();
        }

        return droit;
    }
    
    public async Task<Droit> GetDroitByReferenceAsync(string reference)
    {
        var droit = await Context.Droits
                                .FirstOrDefaultAsync(d => d.Reference.ToLower().Equals(reference.ToLower()));
        if ( droit == null )
        {
            throw new DroitNotFoundException();
        }

        return droit;
    }
    

    public async Task<int> GetYearDroitCount()
    {
        var currentYear = DateTime.UtcNow.Year;
        var referenceEnding = $"/{currentYear.ToString().Substring(2)}";
        return await Context.Droits.CountAsync(d => d.Reference.EndsWith(referenceEnding));
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
                !string.IsNullOrEmpty(d.Reference) && d.Reference.ToLower().Contains(query)||
                (d.Wreck != null && !string.IsNullOrEmpty(d.Wreck.Name) && d.Wreck.Name.ToLower().Contains(query)) ||
                (d.Salvor != null && !string.IsNullOrEmpty(d.Salvor.Name) && d.Salvor.Name.ToLower().Contains(query)) ||
                (d.AssignedToUser != null && !string.IsNullOrEmpty(d.AssignedToUser.Name) && d.AssignedToUser.Name.ToLower().Contains(query))
            )
            .ToListAsync();

        var results = droits.Select(d => new DroitExportDto(d)).ToList();

        return results;
    }
    
    public async Task<Droit> GetDroitByPowerappsIdAsync(string powerappsId)
    {
        var droit = await Context.Droits
            .FirstOrDefaultAsync(w => w.PowerappsDroitId == powerappsId);
        if ( droit == null )
        {
            throw new DroitNotFoundException();
        }

        return droit;
    }
}