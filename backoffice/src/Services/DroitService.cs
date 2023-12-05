
#region

using AutoMapper;
using Droits.Exceptions;
using Droits.Helpers;
using Droits.Helpers.Extensions;
using Droits.Models.DTOs;
using Droits.Models.DTOs.Exports;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Models.FormModels;
using Droits.Models.FormModels.SearchFormModels;
using Droits.Models.ViewModels;
using Droits.Models.ViewModels.ListViews;
using Droits.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

#endregion

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
    Task<List<DroitExportDto>> SearchDroitsAsync(string query);
    Task<DroitListView> AdvancedSearchDroitsAsync(DroitSearchForm form);
    Task<Droit> CreateDroitAsync(SubmittedReportDto report, Salvor salvor);
    Task<byte[]> ExportAsync(DroitSearchForm form);
}

public class DroitService : IDroitService
{
    private readonly IDroitRepository _repo;
    private readonly IWreckMaterialService _wreckMaterialService;
    private readonly IAccountService _accountService;
    private readonly ILogger<DroitService> _logger;
    private readonly IMapper _mapper;


    public DroitService(ILogger<DroitService> logger, IDroitRepository repo,
        IWreckMaterialService wreckMaterialService, IAccountService accountService, IMapper mapper)
    {
        _logger = logger;
        _repo = repo;
        _accountService = accountService;
        _wreckMaterialService = wreckMaterialService;
        _mapper = mapper;
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
        if ( droit.Id != default ) return await UpdateDroitAsync(droit);
        
        return await AddDroitAsync(droit);
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

            var salvorAddress = droit.Salvor?.Address;

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
            _logger.LogError($"Droit not found - {e}");
        }
    }
    
    public async Task UpdateDroitStatusAsync(Guid id, DroitStatus status)
    {
        var droit = await GetDroitAsync(id);

        droit.Status = status;

        await _repo.UpdateAsync(droit);
    }
    
    public async Task<List<DroitExportDto>> SearchDroitsAsync(string query) => await _repo.SearchDroitsAsync(query);
    
    private IQueryable<Droit> QueryFromForm(DroitSearchForm form)
    {
        var query = _repo.GetDroitsWithAssociations();
            //Droit Report Filters
            
            if (!string.IsNullOrEmpty(form.Reference))
            {
                query = query.Where(d =>
                    !string.IsNullOrEmpty(d.Reference) &&
                    EF.Functions.ILike(d.Reference, $"%{form.Reference}%")
                );
            }
            
            if (form.CreatedFrom != null && form.CreatedTo != null)
            {
                query = query.Where(d =>
                    d.Created >= form.CreatedFrom && d.Created <= form.CreatedTo
                );
            }
            
            if (form.LastModifiedFrom != null && form.LastModifiedTo != null)
            {
                query = query.Where(d =>
                    d.LastModified >= form.LastModifiedFrom && d.LastModified <= form.LastModifiedTo
                );
            }
            
            if (form.ReportedDateFrom != null && form.ReportedDateTo != null)
            {
                query = query.Where(d =>
                    d.ReportedDate >= form.ReportedDateFrom && d.ReportedDate <= form.ReportedDateTo
                );
            }
            
            if (form.DateFoundFrom != null && form.DateFoundTo != null)
            {
                query = query.Where(d =>
                    d.DateFound >= form.DateFoundFrom && d.DateFound <= form.DateFoundTo
                );
            }
            
            query = query.Where(d =>
                (form.IsHazardousFind == null || d.IsHazardousFind == form.IsHazardousFind) &&
                (form.IsDredge == null || d.IsDredge == form.IsDredge) &&
                (form.IsIsolatedFind == null || d.WreckId == null == form.IsIsolatedFind) &&
                (form.AssignedToUserId == null || d.AssignedToUserId == form.AssignedToUserId)
            );
            query = query.Where(d =>
                ( form.StatusList.IsNullOrEmpty() ||
                  form.StatusList.Contains(d.Status) ));
            //Wreck Filters
            
            if (!string.IsNullOrEmpty(form.WreckName))
            {
                query = query.Where(d =>
                    d.Wreck != null &&
                    !string.IsNullOrEmpty(d.Wreck.Name) &&
                    EF.Functions.ILike(d.Wreck.Name, $"%{form.WreckName}%")
                );
            }

            query = query.Where(d =>
                !form.WreckName.HasValue() ||
                d.Wreck != null &&
                d.Wreck.Name.HasValue()
            );
            //Salvor Filters
            
            if (!string.IsNullOrEmpty(form.SalvorName))
            {
                query = query.Where(d =>
                    d.Salvor != null &&
                    !string.IsNullOrEmpty(d.Salvor.Name) &&
                    EF.Functions.ILike(d.Salvor.Name, $"%{form.SalvorName}%")
                );
            }

            query = query.Where(d =>
                !form.SalvorName.HasValue() ||
                ( d.Salvor != null &&
                  d.Salvor.Name.HasValue() ));
            //Location Filters
            
            if (form.LatitudeFrom != null && form.LatitudeTo != null)
            {
                query = query.Where(d =>
                    d.Latitude >= form.LatitudeFrom && d.Latitude <= form.LatitudeTo
                );
            }
            if (form.LongitudeFrom != null && form.LongitudeTo != null)
            {
                query = query.Where(d =>
                    d.Longitude >= form.LongitudeFrom && d.Longitude <= form.LongitudeTo
                );
            }
            if (form.DepthFrom != null && form.DepthTo != null)
            {
                query = query.Where(d =>
                    d.Depth >= form.DepthFrom && d.Depth <= form.DepthTo
                );
            }
            
            if (!string.IsNullOrEmpty(form.LocationDescription))
            {
                query = query.Where(d =>
                    d.Salvor != null &&
                    !string.IsNullOrEmpty(d.LocationDescription) &&
                    EF.Functions.ILike(d.LocationDescription, $"%{form.LocationDescription}%")
                );
            }


            query = query.Where(
                    d => // long and lat to use location radius in calculation (method on droit data)
                        ( form.InUkWaters == null || d.InUkWaters == form.InUkWaters ) &&
                        ( form.RecoveredFromList.IsNullOrEmpty() ||
                          ( d.RecoveredFrom.HasValue &&
                            form.RecoveredFromList.Contains(d.RecoveredFrom.Value) ) )
                )
                //Wreck Material Filters
                .Where(d =>
                    form.IgnoreWreckMaterialSearch ||
                    d.WreckMaterials.Any(wm =>
                        form.WreckMaterial != null &&
                        !string.IsNullOrEmpty($"{wm.Name} {wm.Description}") &&
                        ( EF.Functions.ILike(wm.Name, $"%{form.WreckMaterial}%") ||
                          EF.Functions.ILike(wm.Description, $"%{form.WreckMaterial}%") )) &&
                    d.WreckMaterials.Any(wm =>
                        form.WreckMaterialOwner != null &&
                        !string.IsNullOrEmpty(wm.WreckMaterialOwner) &&
                        EF.Functions.ILike(wm.WreckMaterialOwner,
                            $"%{form.WreckMaterialOwner}%")) &&
                    d.WreckMaterials.Any(wm =>
                        form.ValueConfirmed == null || wm.ValueConfirmed == form.ValueConfirmed) &&
                    // the "in between" values here may need to be pull put to an if statement using the first conditional
                    d.WreckMaterials.Any(wm =>
                        ( form.QuantityFrom != null && form.QuantityTo != null ) &&
                        wm.Quantity >= form.QuantityFrom && wm.Quantity <= form.QuantityTo) &&
                    d.WreckMaterials.Any(wm =>
                        ( form.ValueFrom != null && form.ValueTo != null ) &&
                        wm.Value >= form.ValueFrom && wm.Value <= form.ValueTo) &&
                    d.WreckMaterials.Any(wm =>
                        ( form.ReceiverValuationFrom != null &&
                          form.ReceiverValuationTo != null ) &&
                        wm.ReceiverValuation >= form.ReceiverValuationFrom &&
                        wm.ReceiverValuation <= form.ReceiverValuationTo)
                );
            //Salvage Filters
            
            if (!string.IsNullOrEmpty(form.ServicesDescription))
            {
                query = query.Where(d =>
                    d.Salvor != null &&
                    !string.IsNullOrEmpty(d.ServicesDescription) &&
                    EF.Functions.ILike(d.ServicesDescription, $"%{form.ServicesDescription}%")
                );
            }
            
            if (!string.IsNullOrEmpty(form.ServicesDuration))
            {
                query = query.Where(d =>
                    d.Salvor != null &&
                    !string.IsNullOrEmpty(d.ServicesDuration) &&
                    EF.Functions.ILike(d.ServicesDuration, $"%{form.ServicesDuration}%")
                );
            }
            
            if (form.ServicesEstimatedCostFrom != null && form.ServicesEstimatedCostTo != null)
            {
                query = query.Where(d =>
                    d.ServicesEstimatedCost >= form.ServicesEstimatedCostFrom && d.ServicesEstimatedCost <= form.ServicesEstimatedCostTo
                );
            }
            
            if (form.SalvageClaimAwardedFrom != null && form.SalvageClaimAwardedTo != null)
            {
                query = query.Where(d =>
                    d.SalvageClaimAwarded >= form.SalvageClaimAwardedFrom && d.SalvageClaimAwarded <= form.SalvageClaimAwardedTo
                );
            }

            query = query.Where(d =>
                ( form.SalvageAwardClaimed == null ||
                  d.SalvageAwardClaimed == form.SalvageAwardClaimed ) &&
                ( form.MMOLicenceRequired == null ||
                  d.MMOLicenceRequired == form.MMOLicenceRequired ) &&
                ( form.MMOLicenceProvided == null ||
                  d.MMOLicenceProvided == form.MMOLicenceProvided )
            );
            //Legacy Filters
            
            if (!string.IsNullOrEmpty(form.District))
            {
                query = query.Where(d =>
                    d.Salvor != null &&
                    !string.IsNullOrEmpty(d.District) &&
                    EF.Functions.ILike(d.District, $"%{form.District}%")
                );
            }
            
            if (!string.IsNullOrEmpty(form.LegacyFileReference))
            {
                query = query.Where(d =>
                    d.Salvor != null &&
                    !string.IsNullOrEmpty(d.LegacyFileReference) &&
                    EF.Functions.ILike(d.LegacyFileReference, $"%{form.LegacyFileReference}%")
                );
            }
            
            if (!string.IsNullOrEmpty(form.GoodsDischargedBy))
            {
                query = query.Where(d =>
                    d.Salvor != null &&
                    !string.IsNullOrEmpty(d.GoodsDischargedBy) &&
                    EF.Functions.ILike(d.GoodsDischargedBy, $"%{form.GoodsDischargedBy}%")
                );
            }
            
            if (!string.IsNullOrEmpty(form.DateDelivered))
            {
                query = query.Where(d =>
                    d.Salvor != null &&
                    !string.IsNullOrEmpty(d.DateDelivered) &&
                    EF.Functions.ILike(d.DateDelivered, $"%{form.DateDelivered}%")
                );
            }
            
            if (!string.IsNullOrEmpty(form.Agent))
            {
                query = query.Where(d =>
                    d.Salvor != null &&
                    !string.IsNullOrEmpty(d.Agent) &&
                    EF.Functions.ILike(d.Agent, $"%{form.Agent}%")
                );
            }
            
            if (!string.IsNullOrEmpty(form.RecoveredFromLegacy))
            {
                query = query.Where(d =>
                    d.Salvor != null &&
                    !string.IsNullOrEmpty(d.RecoveredFromLegacy) &&
                    EF.Functions.ILike(d.RecoveredFromLegacy, $"%{form.RecoveredFromLegacy}%")
                );
            }
            
            query = query.Where(d =>
                (form.ImportedFromLegacy == null || d.ImportedFromLegacy == form.ImportedFromLegacy) 
            );
        
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
        
        var droitsData = droits.Select(d => new DroitExportDto(d)).ToList();
        
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


    public async Task<Droit> CreateDroitAsync(SubmittedReportDto report, Salvor salvor)
    {
        var droit = _mapper.Map<Droit>(report);

        droit.Reference = await GetNextDroitReference();
        droit.Salvor = salvor;
        droit.SalvorId = salvor.Id;
        droit.OriginalSubmission = JsonConvert.SerializeObject(report);

        return await SaveDroitAsync(droit);
    }
}