#region

using System.Linq.Expressions;
using Droits.Data;
using Droits.Exceptions;
using Droits.Models.DTOs.Exports;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Models.ViewModels.ListViews;
using Droits.Services;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Droits.Repositories;

public interface IDroitRepository
{
    IQueryable<Droit> GetDroits();
    IQueryable<Droit> GetDroitsWithAssociations();
    IQueryable<Droit> GetDroitsForExport();
    IAsyncEnumerable<Droit> StreamDroitsForExport(IQueryable<Droit> query);
    IQueryable<Droit> GetOrderedDroitsWithAssociations(Expression<Func<Droit, object>> orderColumnExpression, bool orderDescending);
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
        return Context.Droits.OrderByDescending(d => d.ReportedDate).ThenByDescending(d => d.Id)
            .Include(d => d.AssignedToUser).AsNoTracking()
            .Include(d => d.Letters).AsNoTracking()
            .Include(d => d.Wreck).AsNoTracking()
            .Include(d => d.Salvor).AsNoTracking()
            .Include(d => d.WreckMaterials.OrderBy(wm => wm.Created)).AsNoTracking();
    }

    public IQueryable<Droit> GetDroitsForExport()
    {
        return Context.Droits.AsNoTracking();
    }

    public IAsyncEnumerable<Droit> StreamDroitsForExport(IQueryable<Droit> query)
    {
        return query.OrderByDescending(d => d.ReportedDate).ThenByDescending(d => d.Id)
            .Select(d => new Droit
            {
                Id = d.Id,
                Reference = d.Reference,
                Created = d.Created,
                LastModified = d.LastModified,
                Status = d.Status,
                TriageNumber = d.TriageNumber,
                ReportedDate = d.ReportedDate,
                DateFound = d.DateFound,
                ClosedDate = d.ClosedDate,
                IsHazardousFind = d.IsHazardousFind,
                IsDredge = d.IsDredge,
                ReportedWreckName = d.ReportedWreckName,
                ReportedWreckYearSunk = d.ReportedWreckYearSunk,
                ReportedWreckYearConstructed = d.ReportedWreckYearConstructed,
                ReportedWreckConstructionDetails = d.ReportedWreckConstructionDetails,
                Latitude = d.Latitude,
                Longitude = d.Longitude,
                InUkWaters = d.InUkWaters,
                LocationRadius = d.LocationRadius,
                Depth = d.Depth,
                RecoveredFrom = d.RecoveredFrom,
                LocationDescription = d.LocationDescription,
                SalvageAwardClaimed = d.SalvageAwardClaimed,
                ServicesDescription = d.ServicesDescription,
                ServicesDuration = d.ServicesDuration,
                ServicesEstimatedCost = d.ServicesEstimatedCost,
                MmoLicenceRequired = d.MmoLicenceRequired,
                MmoLicenceProvided = d.MmoLicenceProvided,
                SalvageClaimAwarded = d.SalvageClaimAwarded,
                District = d.District,
                LegacyFileReference = d.LegacyFileReference,
                GoodsDischargedBy = d.GoodsDischargedBy,
                DateDelivered = d.DateDelivered,
                Agent = d.Agent,
                RecoveredFromLegacy = d.RecoveredFromLegacy,
                LegacyRemarks = d.LegacyRemarks,
                ImportedFromLegacy = d.ImportedFromLegacy,
                Wreck = d.Wreck == null ? null : new Wreck
                {
                    Name = d.Wreck.Name,
                    OwnerName = d.Wreck.OwnerName,
                    OwnerEmail = d.Wreck.OwnerEmail
                },
                Salvor = d.Salvor == null ? null : new Salvor { Name = d.Salvor.Name },
                AssignedToUser = d.AssignedToUser == null ? null : new ApplicationUser { Name = d.AssignedToUser.Name },
                WreckMaterials = d.WreckMaterials.OrderBy(wm => wm.Created)
                    .Select(wm => new WreckMaterial
                    {
                        Name = wm.Name,
                        Description = wm.Description,
                        Quantity = wm.Quantity,
                        SalvorValuation = wm.SalvorValuation,
                        Outcome = wm.Outcome,
                        WreckMaterialOwner = wm.WreckMaterialOwner
                    }).ToList()
            })
            .AsAsyncEnumerable();
    }
    
    public IQueryable<Droit> GetOrderedDroitsWithAssociations(Expression<Func<Droit, object>> orderColumnExpression, bool orderDescending)
    {
        var query = orderDescending
            ? Context.Droits.OrderByDescending(orderColumnExpression).ThenByDescending(d => d.Id)
            : Context.Droits.OrderBy(orderColumnExpression).ThenByDescending(d => d.Id);
        
        return query
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