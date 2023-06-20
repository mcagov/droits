using    Droits.Models;
using Droits.Repositories;

namespace Droits.Services;

public interface IDroitService
{
    Task<List<Droit>> GetDroitsAsync();
    Task<Droit> SaveDroitAsync(Droit droit);
    Task<Droit> GetDroitAsync(Guid id);
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
        if(droit.Id == default(Guid)){
            return await AddDroitAsync(droit);
        }

        return await UpdateDroitAsync(droit);
    }
    private async Task<Droit> AddDroitAsync(Droit droit) =>
        await _repo.AddDroitAsync(droit);

    private async Task<Droit> UpdateDroitAsync(Droit droit) =>
        await _repo.UpdateDroitAsync(droit);

    public async Task<Droit> GetDroitAsync(Guid id) =>
        await _repo.GetDroitAsync(id);

}
