
#region

using AutoMapper;
using CsvHelper;
using Droits.Data.Mappers;
using Droits.Data.Mappers.CsvMappers;
using Droits.Exceptions;
using Droits.Helpers;
using Droits.Helpers.SearchHelpers;
using Droits.Models.DTOs;
using Droits.Models.DTOs.Exports;
using Droits.Models.DTOs.Imports;
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
    Task<Droit> GetDroitByPowerappsIdAsync(string powerappsId);
    Task<List<Droit>> GetDroitsWithAssociationsAsync();
    Task<Droit> SaveDroitAsync(Droit droit);
    Task<Droit> GetDroitAsync(Guid id);
    Task<Droit> AddDroitAsync(Droit droit, bool updateLastModified = true);
    Task<Droit> GetDroitWithAssociationsAsync(Guid id);
    Task SaveWreckMaterialsAsync(Guid id, List<WreckMaterialForm> wreckMaterialForms);
    Task UpdateDroitStatusAsync(Guid id, DroitStatus status);
    Task<string> GetNextDroitReference();
    Task<List<DroitExportDto>> SearchDroitsAsync(string query);
    Task<DroitListView> AdvancedSearchDroitsAsync(DroitSearchForm form);
    Task<Droit> CreateDroitAsync(SubmittedReportDto report, Salvor salvor);
    Task<byte[]> ExportAsync(DroitSearchForm form);
    Task<Droit> GetDroitByReferenceAsync(string reference);
    List<String> UploadWmCsvForm(List<WMRowDto> wreckMaterials);

    Task<List<object>?> GetDroitsMetrics();
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
        var currentYear = DateTime.UtcNow.Year;
        var referenceEnding = $"/{currentYear.ToString().Substring(2)}";
        var nextReferenceNumber = (yearCount + 1).ToString().PadLeft(3, '0');
        return $"{nextReferenceNumber}{referenceEnding}";
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

    public async Task<Droit> GetDroitByPowerappsIdAsync(string powerappsId)
    {
        return await _repo.GetDroitByPowerappsIdAsync(powerappsId);
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


    public async Task<Droit> AddDroitAsync(Droit droit, bool updateLastModified = true)
    {
        if ( string.IsNullOrEmpty(droit.Reference) || !await IsReferenceUnique(droit))
        {
            droit.Reference = await GetNextDroitReference();
        }

        return await _repo.AddAsync(droit, updateLastModified);
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
        
        wreckMaterialForms = wreckMaterialForms.Where(wmf => !string.IsNullOrEmpty(wmf.Name)).ToList();
        var wreckMaterialIdsToKeep = wreckMaterialForms.Where(wm => !string.IsNullOrEmpty(wm.Name)).Select(wm => wm.Id);

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
        return DroitQueryBuilder.BuildQuery(form,query);
    }


    private List<Droit> SearchDroits(IQueryable<Droit> query)
    {
        return query.ToList();
    }

    public async Task<byte[]> ExportAsync(DroitSearchForm form)
    {
        var query = QueryFromForm(form);

        var droits = SearchDroits(query);

         
        if (droits.IsNullOrEmpty())
        {
            throw new Exception("No Droits to export");
        }
        
        
        
        try
        {
            var droitsData = droits.Select(d => new DroitExportDto(d)).ToList();
            return await ExportHelper.ExportRecordsAsync(droitsData, new DroitsCsvMap(form.DroitExportForm));
        }
        catch ( Exception e )
        {
            Console.WriteLine(e);
            throw;
        }

    }

    public async Task<Droit> GetDroitByReferenceAsync(string reference)
    {
        return await _repo.GetDroitByReferenceAsync(reference);
    }


    public List<String> UploadWmCsvForm(List<WMRowDto> wreckMaterials)
    {
        var result = new List<WreckMaterialForm>();

        foreach ( var rowDto in wreckMaterials )
        {
            // Enum.TryParse<WreckMaterialOutcome>(rowDto.Outcome,out var outcome);
            var wmForm = new WreckMaterialForm();
            // wmForm.DroitId = new Guid(rowDto.DroitId);
            wmForm.Name = rowDto.Name;
            // wmForm.StoredAtSalvorAddress = rowDto.StoredAtSalvorAddress.AsBoolean();
            // wmForm.Description = rowDto.Description;
            // wmForm.Quantity = Convert.ToInt32(rowDto.Quantity);
            // wmForm.SalvorValuation = Convert.ToDouble(rowDto.SalvorValuation);
            // wmForm.ReceiverValuation =Convert.ToDouble(rowDto.ReceiverValuation);
            // wmForm.ValueConfirmed = rowDto.ValueConfirmed.AsBoolean();
            // wmForm.WreckMaterialOwner = rowDto.WreckMaterialOwner;
            // wmForm.WreckMaterialOwnerContactDetails = rowDto.WreckMaterialOwnerContactDetails;
            // wmForm.Purchaser = rowDto.Purchaser;
            // wmForm.PurchaserContactDetails = rowDto.PurchaserContactDetails;
            // wmForm.Outcome = outcome;
            // wmForm.OutcomeRemarks = rowDto.OutcomeRemarks;
            // wmForm.WhereSecured = rowDto.WhereSecured;
            
            result.Add(wmForm);
        }

        return result.Select(wm => wm.Name).ToList();
    }


    public async Task<DroitListView> AdvancedSearchDroitsAsync(DroitSearchForm form)
    {
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

    public async Task<List<object>?> GetDroitsMetrics()
    {
        var allDroits = await GetDroitsAsync();

        return (MetricsHelper.GetDroitsMetrics(allDroits) ?? Array.Empty<object>()).ToList();

    }


    
}