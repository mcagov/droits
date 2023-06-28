using Droits.Models.Entities;
using Droits.Repositories;

namespace Droits.Services;

public interface ISalvorService
{
    Task<List<Salvor>> GetSalvorsAsync();
    Task<Salvor> SaveSalvorAsync(Salvor salvor);
    Task<Salvor> GetSalvorAsync(Guid id);
}

public class SalvorService : ISalvorService
{
    public readonly ISalvorRepository _repo;

    public SalvorService(ISalvorRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<Salvor>> GetSalvorsAsync()
    {
        return await _repo.GetSalvorsAsync();
    }

    public async Task<Salvor> SaveSalvorAsync(Salvor salvor)
    {
        if(salvor.Id == default(Guid)){
            return await AddSalvorAsync(salvor);
        }

        return await UpdateSalvorAsync(salvor);
    }
    private async Task<Salvor> AddSalvorAsync(Salvor salvor) =>
        await _repo.AddSalvorAsync(salvor);

    private async Task<Salvor> UpdateSalvorAsync(Salvor salvor) =>
        await _repo.UpdateSalvorAsync(salvor);

    public async Task<Salvor> GetSalvorAsync(Guid id) =>
        await _repo.GetSalvorAsync(id);

}
