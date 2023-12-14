
#region

using AutoMapper;
using Droits.Exceptions;
using Droits.Helpers;
using Droits.Helpers.Extensions;
using Droits.Helpers.SearchHelpers;
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