using System.Globalization;
using System.Text;
using Droits.Exceptions;
using Droits.Helpers;
using Droits.Models.DTOs;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Models.Enums;
using Droits.Models.ViewModels;
using Droits.Models.ViewModels.ListViews;
using Droits.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using CsvHelper;
using CsvHelper.Configuration;

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
    Task<byte[]> ExportDroitsAsync(List<Droit> droits); //return type might be wrong. 
}

public class DroitService : IDroitService
{
    private readonly IDroitRepository _repo;
    private readonly IWreckMaterialService _wreckMaterialService;
    private readonly IAccountService _accountService;
    private readonly ILogger<DroitService> _logger;

    


    public DroitService(ILogger<DroitService> logger, IDroitRepository repo, IWreckMaterialService wreckMaterialService, IAccountService accountService)
    {
        _logger = logger;
        _repo = repo;
        _accountService = accountService;
        _wreckMaterialService = wreckMaterialService;
    }


    public async Task<string> GetNextDroitReference()
    {
        var yearCount = await _repo.GetYearDroitCount();
        
        return  $"{(yearCount+1):D3}/{DateTime.UtcNow:yy}";
    }
    public async Task<DroitListView> GetDroitsListViewAsync(SearchOptions searchOptions)
    {
        var query = searchOptions.IncludeAssociations
            ? _repo.GetDroitsWithAssociations()
            : _repo.GetDroits();
        
        if (searchOptions.FilterByAssignedUser)
        {
            var currentUserId = _accountService.GetCurrentUserId();

            query = query.Where(d => d.AssignedToUserId.HasValue && d.AssignedToUserId == currentUserId);
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
        if (!await IsReferenceUnique(droit))
        {
            throw new DuplicateDroitReferenceException($"Droit Reference {droit.Reference} already exists");
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

        await _wreckMaterialService.DeleteWreckMaterialForDroitAsync(droitId, wreckMaterialIdsToKeep);

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

    public async Task<byte[]> ExportDroitsAsync(List<Droit> droits)
    {
        if (droits.IsNullOrEmpty())
        {
            throw new Exception("No Droits to export");
        }
    
        var droitsData = droits.Select(d => new DroitDto(d)).ToList();

        var csvData = new StringWriter();
        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
        var csv = new CsvWriter(csvData, csvConfiguration);
    
        await csv.WriteRecordsAsync(droitsData);
    
        byte[] fileContents = Encoding.UTF8.GetBytes(csvData.ToString());
    
        return fileContents;
    }


}