#region

using Droits.Helpers;
using Droits.Models.DTOs;
using Droits.Models.DTOs.Exports;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Models.FormModels.SearchFormModels;
using Droits.Models.ViewModels;
using Droits.Models.ViewModels.ListViews;
using Droits.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

#endregion

namespace Droits.Services;

public interface IWreckService
{
    Task<List<Wreck>> GetWrecksAsync();
    Task<Wreck> SaveWreckAsync(Wreck wreck);
    Task<Wreck> GetWreckAsync(Guid id);
    Task<Guid> SaveWreckFormAsync(WreckForm wreckForm);
    Task<WreckListView> GetWrecksListViewAsync(SearchOptions searchOptions);
    Task<WreckListView> AdvancedSearchAsync(WreckSearchForm form);
    Task<byte[]> ExportAsync(WreckSearchForm form);
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
    
    public async Task<WreckListView> AdvancedSearchAsync(WreckSearchForm form)
    {
        var query = QueryFromForm(form)
            .Select(w => new WreckView(w, true));
        
        var pagedResults =
            await ServiceHelper.GetPagedResult(query, form);

        return new WreckListView(pagedResults.Items)
        {
            PageNumber = pagedResults.PageNumber,
            PageSize = pagedResults.PageSize,
            IncludeAssociations = pagedResults.IncludeAssociations,
            TotalCount = pagedResults.TotalCount,
            SearchForm = form
        };
    }

    private IQueryable<Wreck> QueryFromForm(WreckSearchForm form)
    {
        var query = _repo.GetWrecksWithAssociations()
            .OrderByDescending(w => w.Created)
            .Where(w =>
                SearchHelper.FuzzyMatches(form.Name, w.Name, 70) 
            );

        return query;
    }


    private List<Wreck> SearchWrecks(IQueryable<Wreck> query)
    {
        return query.ToList();
    }
    public async Task<byte[]> ExportAsync(WreckSearchForm form)
    {
        var query = QueryFromForm(form);

        var wrecks = SearchWrecks(query);
        
        var wrecksData = wrecks.Select(s => new WreckExportDto(s)).ToList();
        
        if (wrecks.IsNullOrEmpty())
        {
            throw new Exception("No Wrecks to export");
        }

        return await ExportHelper.ExportRecordsAsync(wrecksData);
    }
}