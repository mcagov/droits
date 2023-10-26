using Droits.Helpers;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Models.ViewModels;
using Droits.Models.ViewModels.ListViews;
using Droits.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Droits.Services;

public interface ISalvorService
{
    Task<SalvorListView> GetSalvorListViewAsync(SearchOptions searchOptions);
    Task<List<Salvor>> GetSalvorsAsync();
    Task<Salvor> SaveSalvorAsync(Salvor salvor);
    Task<Salvor> GetSalvorAsync(Guid id);
    Task<Guid> SaveSalvorFormAsync(SalvorForm form);
    Task<SalvorListView> SearchSalvorsAsync(SalvorSearchForm form, SearchOptions searchOptions);
}

public class SalvorService : ISalvorService
{
    private readonly ISalvorRepository _repo;


    public SalvorService(ISalvorRepository repo)
    {
        _repo = repo;
    }


    public async Task<SalvorListView> GetSalvorListViewAsync(SearchOptions searchOptions)
    {
        var query = searchOptions.IncludeAssociations
            ? _repo.GetSalvorsWithAssociations()
            : _repo.GetSalvors();
        var pagedItems = await ServiceHelper.GetPagedResult(
            query.Select(s => new SalvorView(s, searchOptions.IncludeAssociations)), searchOptions);

        return new SalvorListView(pagedItems.Items)
        {
            PageNumber = pagedItems.PageNumber,
            PageSize = pagedItems.PageSize,
            IncludeAssociations = pagedItems.IncludeAssociations,
            TotalCount = pagedItems.TotalCount
        };
    }


    public async Task<List<Salvor>> GetSalvorsAsync()
    {
        return await _repo.GetSalvors().ToListAsync();
    }


    public async Task<Salvor> SaveSalvorAsync(Salvor salvor)
    {
        if ( salvor.Id == default )
        {
            return await AddSalvorAsync(salvor);
        }

        return await UpdateSalvorAsync(salvor);
    }


    private async Task<Salvor> AddSalvorAsync(Salvor salvor)
    {
        return await _repo.AddAsync(salvor);
    }


    private async Task<Salvor> UpdateSalvorAsync(Salvor salvor)
    {
        return await _repo.UpdateAsync(salvor);
    }


    public async Task<Salvor> GetSalvorAsync(Guid id)
    {
        return await _repo.GetSalvorAsync(id);
    }


    public async Task<Guid> SaveSalvorFormAsync(SalvorForm form)
    {
        var salvor = form.ApplyChanges(new Salvor());

        return ( await SaveSalvorAsync(salvor) ).Id;
    }


    public async Task<SalvorListView> SearchSalvorsAsync(SalvorSearchForm form, SearchOptions searchOptions)
    {
        var query = _repo.GetSalvorsWithAssociations()
            .OrderByDescending(s => s.LastModified)
            .Where(s =>
                SearchHelper.Matches(form.Name, s.Name)
            )
            .Select(s => new SalvorView(s, true));
        
        var pagedSalvors =
            await ServiceHelper.GetPagedResult(query, searchOptions);

        return new SalvorListView(pagedSalvors.Items)
        {
            PageNumber = pagedSalvors.PageNumber,
            PageSize = pagedSalvors.PageSize,
            IncludeAssociations = pagedSalvors.IncludeAssociations,
            TotalCount = pagedSalvors.TotalCount,
            SearchForm = form
        };
    }
}