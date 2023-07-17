using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Repositories;

namespace Droits.Services;

public interface IWreckService
{
    Task<List<Wreck>> GetWrecksAsync();
    Task<Wreck> SaveWreckAsync(Wreck wreck);
    Task<Wreck> GetWreckAsync(Guid id);
    Task<Guid> SaveWreckFormAsync(WreckForm wreckForm);
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
        if ( wreck.Id == default )
        {
            return await AddWreckAsync(wreck);
        }

        return await UpdateWreckAsync(wreck);
    }


    private async Task<Wreck> AddWreckAsync(Wreck wreck)
    {
        return await _repo.AddWreckAsync(wreck);
    }


    private async Task<Wreck> UpdateWreckAsync(Wreck wreck)
    {
        return await _repo.UpdateWreckAsync(wreck);
    }


    public async Task<Wreck> GetWreckAsync(Guid id)
    {
        return await _repo.GetWreckAsync(id);
    }


    public async Task<Guid> SaveWreckFormAsync(WreckForm wreckForm)
    {
        var wreck = wreckForm.ApplyChanges(new Wreck());

        return ( await SaveWreckAsync(wreck) ).Id;
    }
}