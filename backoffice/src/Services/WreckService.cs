
using Droits.Models;
using Droits.Repositories;

namespace Droits.Services;

public interface IWreckService
{
    Task<List<Wreck>> GetWrecksAsync();
    Task<Wreck> SaveWreckAsync(Wreck wreck);
    Task<Wreck> GetWreckAsync(Guid id);
}

public class WreckService : IWreckService
{
    public readonly IWreckRepository _repo;

    public WreckService(IWreckRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<Wreck>> GetWrecksAsync()
    {
        return await _repo.GetWrecksAsync();
    }

    public async Task<Wreck> SaveWreckAsync(Wreck wreck)
    {
        if(wreck.Id == default(Guid)){
            return await AddWreckAsync(wreck);
        }

        return await UpdateWreckAsync(wreck);
    }
    private async Task<Wreck> AddWreckAsync(Wreck wreck) =>
        await _repo.AddWreckAsync(wreck);

    private async Task<Wreck> UpdateWreckAsync(Wreck wreck) =>
        await _repo.UpdateWreckAsync(wreck);

    public async Task<Wreck> GetWreckAsync(Guid id) =>
        await _repo.GetWreckAsync(id);

}
