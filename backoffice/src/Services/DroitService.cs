using Droits.Exceptions;
using Droits.Helpers;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Models.Enums;
using Droits.Models.ViewModels;
using Droits.Models.ViewModels.ListViews;
using Droits.Repositories;
using Microsoft.EntityFrameworkCore;

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
}

public class DroitService : IDroitService
{
    private readonly IDroitRepository _repo;
    private readonly IWreckMaterialRepository _wreckMaterialRepo;
    private readonly ILogger<DroitService> _logger;


    public DroitService(ILogger<DroitService> logger, IDroitRepository repo, IWreckMaterialRepository wmRepo)
    {
        _logger = logger;
        _repo = repo;
        _wreckMaterialRepo = wmRepo;
    }


    public async Task<DroitListView> GetDroitsListViewAsync(SearchOptions searchOptions)
    {
        var query = searchOptions.IncludeAssociations
            ? _repo.GetDroitsWithAssociations()
            : _repo.GetDroits();
        var pagedDroits =
            await ServiceHelpers.GetPagedResult(query.Select(d => new DroitView(d)), searchOptions);

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
            return await AddDroitAsync(droit);
        }

        return await UpdateDroitAsync(droit);
    }


    private async Task<Droit> AddDroitAsync(Droit droit)
    {
        return await _repo.AddAsync(droit);
    }


    private async Task<Droit> UpdateDroitAsync(Droit droit)
    {
        return await _repo.UpdateAsync(droit);
    }


    public async Task<Droit> GetDroitWithAssociationsAsync(Guid id)
    {
        return await _repo.GetDroitWithAssociationsAsync(id);
    }


    public async Task<Droit> GetDroitAsync(Guid id)
    {
        return await _repo.GetDroitAsync(id);
    }


    private async Task<WreckMaterial> SaveWreckMaterialAsync(WreckMaterialForm wreckMaterialForm)
    {
        if ( wreckMaterialForm.Id == default )
        {
            return await _wreckMaterialRepo.AddAsync(
                wreckMaterialForm.ApplyChanges(new WreckMaterial()));
        }

        var wreckMaterial =
            await _wreckMaterialRepo.GetWreckMaterialAsync(wreckMaterialForm.Id, wreckMaterialForm.DroitId);

        wreckMaterial = wreckMaterialForm.ApplyChanges(wreckMaterial);

        return await UpdateWreckMaterialAsync(wreckMaterial);
    }


    private async Task<WreckMaterial> UpdateWreckMaterialAsync(WreckMaterial wreckMaterial)
    {
        return await _wreckMaterialRepo.UpdateAsync(wreckMaterial);
    }


    private async Task DeleteWreckMaterialForDroitAsync(Guid droitId, IEnumerable<Guid> wmToKeep)
    {
        await _wreckMaterialRepo.DeleteWreckMaterialForDroitAsync(droitId, wmToKeep);
    }


    public async Task SaveWreckMaterialsAsync(Guid droitId,
        List<WreckMaterialForm> wreckMaterialForms)
    {
        var wreckMaterialIdsToKeep = wreckMaterialForms.Select(wm => wm.Id);

        await DeleteWreckMaterialForDroitAsync(droitId, wreckMaterialIdsToKeep);

        try
        {
            var droit = await GetDroitWithAssociationsAsync(droitId);

            var salvorAddress = droit?.Salvor?.Address;

            foreach ( var wmForm in wreckMaterialForms )
            {
                wmForm.DroitId = droitId;
                if ( salvorAddress != null && wmForm.StoredAtSalvor )
                {
                    wmForm.StorageAddress = new AddressForm(salvorAddress);
                }

                await SaveWreckMaterialAsync(wmForm);
            }
        }
        catch ( DroitNotFoundException e )
        {
            _logger.LogError("Droit not found", e);
        }
    }


    public async Task UpdateDroitStatusAsync(Guid id, DroitStatus status)
    {
        await _repo.UpdateDroitStatusAsync(id, status);
    }
}