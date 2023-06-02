using Droits.Models;
using Droits.Repositories;

namespace Droits.Services;

public interface IDroitService
{
    Task<List<Droit>> GetDroitsAsync();
    Task AddDroitAsync(Droit droit);
    Task<Droit?> GetDroitAsync(Guid id);
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

    public async Task AddDroitAsync(Droit droit)
    {
        await _repo.AddDroitAsync(droit);
    }

    public async Task<Droit?> GetDroitAsync(Guid id)
    {
        return await _repo.GetDroitAsync(id);
    }
}