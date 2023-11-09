using Droits.Exceptions;
using Droits.Helpers;
using Droits.Helpers.Extensions;
using Droits.Models.DTOs;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Models.Enums;
using Droits.Models.FormModels.SearchFormModels;
using Droits.Models.ViewModels;
using Droits.Models.ViewModels.ListViews;
using Droits.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Droits.Services;

public interface IDroitService
{
    Task<DroitListView> GetDroitsListViewAsync(SearchOptions searchOptions);
    Task<List<Droit>> GetDroitsAsync();
    Task<List<Droit>> GetDroitsWithAssociationsAsync();
    Task<Droit> SaveDroitAsync(Droit droit);
    Task<Droit> GetDroitAsync(Guid id);
    Task<Droit> GetDroitWithAssociationsAsync(Guid id);
    Task SaveWreckMaterialsAsync(Guid id, List<WreckMaterialForm> wreckMaterialForms);
    Task UpdateDroitStatusAsync(Guid id, DroitStatus status);
    Task<string> GetNextDroitReference();
    Task<List<DroitDto>> SearchDroitsAsync(string query);
    Task<DroitListView> AdvancedSearchDroitsAsync(DroitSearchForm form);
    Task<byte[]> ExportAsync(DroitSearchForm form); 
}

public class DroitService : IDroitService
{
    private readonly IDroitRepository _repo;
    private readonly IWreckMaterialService _wreckMaterialService;
    private readonly IAccountService _accountService;
    private readonly ILogger<DroitService> _logger;


    public DroitService(ILogger<DroitService> logger, IDroitRepository repo,
        IWreckMaterialService wreckMaterialService, IAccountService accountService)
    {
        _logger = logger;
        _repo = repo;
        _accountService = accountService;
        _wreckMaterialService = wreckMaterialService;
    }


    public async Task<string> GetNextDroitReference()
    {
        var yearCount = await _repo.GetYearDroitCount();

        return $"{( yearCount + 1 ):D3}/{DateTime.UtcNow:yy}";
    }


    public async Task<DroitListView> GetDroitsListViewAsync(SearchOptions searchOptions)
    {
        var query = searchOptions.IncludeAssociations
            ? _repo.GetDroitsWithAssociations()
            : _repo.GetDroits();

        if ( searchOptions.FilterByAssignedUser )
        {
            var currentUserId = _accountService.GetCurrentUserId();

            query = query.Where(d =>
                d.AssignedToUserId.HasValue && d.AssignedToUserId == currentUserId);
        }

        var pagedDroits =
            await ServiceHelper.GetPagedResult(query.Select(d => new DroitView(d)), searchOptions);

        return new DroitListView(pagedDroits.Items)
        {
            PageNumber = pagedDroits.PageNumber,
            PageSize = pagedDroits.PageSize,
            IncludeAssociations = pagedDroits.IncludeAssociations,
            TotalCount = pagedDroits.TotalCount
        };
    }


    public async Task<List<Droit>> GetDroitsAsync()
    {
        return await _repo.GetDroits().ToListAsync();
    }


    public async Task<List<Droit>> GetDroitsWithAssociationsAsync()
    {
        return await _repo.GetDroitsWithAssociations().ToListAsync();
    }


    public async Task<Droit> SaveDroitAsync(Droit droit)
    {
        if ( droit.Id == default )
        {
            droit.Reference = await GetNextDroitReference();
            return await AddDroitAsync(droit);
        }

        return await UpdateDroitAsync(droit);
    }


    private async Task<Droit> AddDroitAsync(Droit droit)
    {
        droit.Reference = await GetNextDroitReference();
        return await _repo.AddAsync(droit);
    }


    private async Task<Droit> UpdateDroitAsync(Droit droit)
    {
        if ( !await IsReferenceUnique(droit) )
        {
            throw new DuplicateDroitReferenceException(
                $"Droit Reference {droit.Reference} already exists");
        }

        return await _repo.UpdateAsync(droit);
    }


    private async Task<bool> IsReferenceUnique(Droit droit) => await _repo.IsReferenceUnique(droit);


    public async Task<Droit> GetDroitWithAssociationsAsync(Guid id)
    {
        return await _repo.GetDroitWithAssociationsAsync(id);
    }


    public async Task<Droit> GetDroitAsync(Guid id)
    {
        return await _repo.GetDroitAsync(id);
    }


    public async Task SaveWreckMaterialsAsync(Guid droitId,
        List<WreckMaterialForm> wreckMaterialForms)
    {
        var wreckMaterialIdsToKeep = wreckMaterialForms.Select(wm => wm.Id);

        await _wreckMaterialService.DeleteWreckMaterialForDroitAsync(droitId,
            wreckMaterialIdsToKeep);

        try
        {
            var droit = await GetDroitWithAssociationsAsync(droitId);

            var salvorAddress = droit?.Salvor?.Address;

            foreach ( var wmForm in wreckMaterialForms )
            {
                wmForm.DroitId = droitId;
                if ( salvorAddress != null && wmForm.StoredAtSalvorAddress )
                {
                    wmForm.StorageAddress = new AddressForm(salvorAddress);
                }

                await _wreckMaterialService.SaveWreckMaterialAsync(wmForm);
            }
        }
        catch ( DroitNotFoundException e )
        {
            _logger.LogError("Droit not found", e);
        }
    }


    public async Task UpdateDroitStatusAsync(Guid id, DroitStatus status)
    {
        var droit = await GetDroitAsync(id);

        droit.Status = status;

        await _repo.UpdateAsync(droit);
    }
    
    public async Task<List<DroitDto>> SearchDroitsAsync(string query) => await _repo.SearchDroitsAsync(query);
    
    private IQueryable<Droit> QueryFromForm(DroitSearchForm form)
    {
        var query = _repo.GetDroitsWithAssociations()
            //Droit Report Filters
            .Where(d =>
                SearchHelper.Matches(form.Reference, d.Reference) &&
                SearchHelper.IsBetween(d.Created, form.CreatedFrom, form.CreatedTo) &&
                SearchHelper.IsBetween(d.LastModified, form.LastModifiedFrom,
                    form.LastModifiedTo) &&
                ( form.StatusList.IsNullOrEmpty() ||
                  form.StatusList.Contains(d.Status) ) &&
                SearchHelper.IsBetween(d.ReportedDate, form.ReportedDateFrom,
                    form.ReportedDateTo) &&
                SearchHelper.IsBetween(d.DateFound, form.DateFoundFrom, form.DateFoundTo) &&
                SearchHelper.Matches(form.IsHazardousFind,
                    d.IsHazardousFind) &&
                SearchHelper.Matches(form.IsDredge, d.IsDredge) &&
                SearchHelper.Matches(form.AssignedToUserId, d.AssignedToUserId))
            //Wreck Filters
            .Where(d =>
                ( !form.WreckName.HasValue() ||
                  ( d.Wreck != null &&
                    d.Wreck.Name.HasValue() &&
                    SearchHelper.Matches(form.WreckName, d.Wreck.Name) ) ) &&
                SearchHelper.Matches(form.IsIsolatedFind, d.WreckId == null)
            )
            //Salvor Filters
            .Where(d =>
                !form.SalvorName.HasValue() ||
                ( d.Salvor != null &&
                  d.Salvor.Name.HasValue() &&
                  SearchHelper.Matches(form.SalvorName, d.Salvor.Name) )
            )
            //Location Filters
            .Where(d =>
                // long and lat to use location radius in calculation (method on droit data)
                SearchHelper.IsBetween(d.Latitude, form.LatitudeFrom, form.LatitudeTo) &&
                SearchHelper.IsBetween(d.Longitude, form.LongitudeFrom, form.LongitudeTo) &&
                SearchHelper.IsBetween(d.Depth, form.DepthFrom, form.DepthTo) &&
                SearchHelper.Matches(form.InUkWaters, d.InUkWaters) &&
                ( form.RecoveredFromList.IsNullOrEmpty() ||
                  ( d.RecoveredFrom.HasValue &&
                    form.RecoveredFromList.Contains(d.RecoveredFrom.Value) ) ) &&
                SearchHelper.Matches(form.LocationDescription, d.LocationDescription)
            )
            //Wreck Material Filters
            .Where(d =>
                form.IgnoreWreckMaterialSearch ||
                d.WreckMaterials.Any(wm =>
                    SearchHelper.Matches(form.WreckMaterial, $"{wm.Name} {wm.Description}")) &&
                d.WreckMaterials.Any(wm =>
                    SearchHelper.Matches(form.WreckMaterialOwner, wm.WreckMaterialOwner)) &&
                d.WreckMaterials.Any(wm =>
                    SearchHelper.Matches(form.ValueConfirmed, wm.ValueConfirmed)) &&
                d.WreckMaterials.Any(wm =>
                    SearchHelper.IsBetween(wm.Quantity, form.QuantityFrom, form.QuantityTo)) &&
                d.WreckMaterials.Any(wm =>
                    SearchHelper.IsBetween(wm.Value, form.ValueFrom, form.ValueTo)) &&
                d.WreckMaterials.Any(wm => SearchHelper.IsBetween(wm.ReceiverValuation,
                    form.ReceiverValuationFrom,
                    form.ReceiverValuationTo))

            )
            //Salvage Filters
            .Where(d =>
                SearchHelper.Matches(form.SalvageAwardClaimed, d.SalvageAwardClaimed) &&
                SearchHelper.Matches(form.ServicesDescription, d.ServicesDescription) &&
                SearchHelper.Matches(form.ServicesDuration, d.ServicesDuration) &&
                SearchHelper.IsBetween(d.ServicesEstimatedCost, form.ServicesEstimatedCostFrom,
                    form.ServicesEstimatedCostTo) &&
                SearchHelper.Matches(form.MMOLicenceRequired, d.MMOLicenceRequired) &&
                SearchHelper.Matches(form.MMOLicenceProvided, d.MMOLicenceProvided) &&
                SearchHelper.IsBetween(d.SalvageClaimAwarded, form.SalvageClaimAwardedFrom,
                    form.SalvageClaimAwardedTo)
            )
            //Legacy Filters
            .Where(d =>
                SearchHelper.Matches(form.District, d.District) &&
                SearchHelper.Matches(form.LegacyFileReference, d.LegacyFileReference) &&
                SearchHelper.Matches(form.GoodsDischargedBy, d.GoodsDischargedBy) &&
                SearchHelper.Matches(form.DateDelivered, d.DateDelivered) &&
                SearchHelper.Matches(form.Agent, d.Agent) &&
                SearchHelper.Matches(form.RecoveredFromLegacy, d.RecoveredFromLegacy) &&
                SearchHelper.Matches(form.ImportedFromLegacy, d.ImportedFromLegacy));
        
        return query;
    }


    private List<Droit> SearchDroits(IQueryable<Droit> query)
    {
        return query.ToList();
    }

    public async Task<byte[]> ExportAsync(DroitSearchForm form)
    {
        var query = QueryFromForm(form);

        var droits = SearchDroits(query);
        
        var droitsData = droits.Select(d => new DroitDto(d)).ToList();
        
        if (droits.IsNullOrEmpty())
        {
            throw new Exception("No Droits to export");
        }

        return await ExportHelper.ExportRecordsAsync(droitsData);
    }

    public async Task<DroitListView> AdvancedSearchDroitsAsync(DroitSearchForm form)
    {
        
        //To-do - move somewhere better/ more generic with other searches. 
        var query = QueryFromForm(form)
            .Select(d => new DroitView(d));


        var pagedDroits =
            await ServiceHelper.GetPagedResult(query, form);

        return new DroitListView(pagedDroits.Items)
        {
            PageNumber = pagedDroits.PageNumber,
            PageSize = pagedDroits.PageSize,
            IncludeAssociations = pagedDroits.IncludeAssociations,
            TotalCount = pagedDroits.TotalCount,
            SearchForm = form
        };
    }
}