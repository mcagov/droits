using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Models.Enums;
using Droits.Repositories;

namespace Droits.Services;

public interface IDroitService
{
    Task<List<Droit>> GetDroitsAsync();
    Task<Droit> SaveDroitAsync(Droit droit);
    Task<Droit> GetDroitAsync(Guid id);
    Task SaveWreckMaterialsAsync(Guid id, List<WreckMaterialForm> wreckMaterialForms);
    Task UpdateDroitStatusAsync(Guid id, DroitStatus status);
}

public class DroitService : IDroitService
{
    public readonly IDroitRepository _repo;


    public DroitService(IDroitRepository repo)
    {
        _repo = repo;
    }


    public async Task<List<Droit>> GetDroitsAsync()
    {
        return await _repo.GetDroitsAsync();
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
        return await _repo.AddDroitAsync(droit);
    }


    private async Task<Droit> UpdateDroitAsync(Droit droit)
    {
        return await _repo.UpdateDroitAsync(droit);
    }


    public async Task<Droit> GetDroitAsync(Guid id)
    {
        return await _repo.GetDroitAsync(id);
    }


    private async Task<WreckMaterial> SaveWreckMaterialAsync(WreckMaterialForm wreckMaterialForm)
    {
        if ( wreckMaterialForm.Id == default )
        {
            return await _repo.AddWreckMaterialAsync(
                wreckMaterialForm.ApplyChanges(new WreckMaterial()));
        }

        var wreckMaterial =
            await _repo.GetWreckMaterialAsync(wreckMaterialForm.Id, wreckMaterialForm.DroitId);

        wreckMaterial = wreckMaterialForm.ApplyChanges(wreckMaterial);

        return await UpdateWreckMaterialAsync(wreckMaterial);
    }


    private async Task<WreckMaterial> UpdateWreckMaterialAsync(WreckMaterial wreckMaterial)
    {
        return await _repo.UpdateWreckMaterialAsync(wreckMaterial);
    }


    private async Task DeleteWreckMaterialForDroitAsync(Guid droitId, IEnumerable<Guid> wmToKeep)
    {
        await _repo.DeleteWreckMaterialForDroitAsync(droitId, wmToKeep);
    }


    public async Task SaveWreckMaterialsAsync(Guid droitId,
        List<WreckMaterialForm> wreckMaterialForms)
    {
        var wreckMaterialIdsToKeep = wreckMaterialForms.Select(wm => wm.Id);

        await DeleteWreckMaterialForDroitAsync(droitId, wreckMaterialIdsToKeep);

        foreach ( var wmForm in wreckMaterialForms )
        {
            wmForm.DroitId = droitId;
            await SaveWreckMaterialAsync(wmForm);
        }
    }


    public async Task UpdateDroitStatusAsync(Guid id, DroitStatus status)
    {
        await _repo.UpdateDroitStatusAsync(id, status);
    }
}