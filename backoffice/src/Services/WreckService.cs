using Droits.Helpers;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Models.ViewModels;
using Droits.Models.ViewModels.ListViews;
using Droits.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Droits.Services;

public interface IWreckService
{
    Task<List<Wreck>> GetWrecksAsync();
    Task<Wreck> SaveWreckAsync(Wreck wreck);
    Task<Wreck> GetWreckAsync(Guid id);
    Task<Guid> SaveWreckFormAsync(WreckForm wreckForm);
    Task<WreckListView> GetWrecksListViewAsync(SearchOptions searchOptions);
}

public class WreckService : IWreckService
{
    private readonly IWreckRepository _repo;


    public WreckService(IWreckRepository repo)
    {
        _repo = repo;
    }


    public async Task<WreckListView> GetWrecksListViewAsync(SearchOptions searchOptions)
    {
        var query = searchOptions.IncludeAssociations
            ? _repo.GetWrecksWithAssociations()
            : _repo.GetWrecks();
        var pagedItems = await ServiceHelper.GetPagedResult(
            query.Select(w => new WreckView(w, searchOptions.IncludeAssociations)), searchOptions);

        return new WreckListView(pagedItems.Items)
        {
            PageNumber = pagedItems.PageNumber,
            PageSize = pagedItems.PageSize,
            IncludeAssociations = pagedItems.IncludeAssociations,
            TotalCount = pagedItems.TotalCount
        };
    }


    public async Task<List<Wreck>> GetWrecksAsync()
    {
        return await _repo.GetWrecks().ToListAsync();
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
        return await _repo.AddAsync(wreck);
    }


    private async Task<Wreck> UpdateWreckAsync(Wreck wreck)
    {
        return await _repo.UpdateAsync(wreck);
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