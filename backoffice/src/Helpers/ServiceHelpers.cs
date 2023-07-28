using Droits.Models.ViewModels;
using Droits.Models.ViewModels.ListViews;
using Microsoft.EntityFrameworkCore;

namespace Droits.Helpers;

public static class ServiceHelpers
{
    public static async Task<ListView<TView>> GetPagedResult<TView>(
        IQueryable<TView> query,
        SearchOptions searchOptions) where TView : class
    {
        var totalCount = await query.CountAsync();
        var pagedItems = await query.Skip(( searchOptions.PageNumber - 1 ) * searchOptions.PageSize)
            .Take(searchOptions.PageSize)
            .ToListAsync();

        return new ListView<TView>
        {
            PageNumber = searchOptions.PageNumber,
            PageSize = searchOptions.PageSize,
            IncludeAssociations = searchOptions.IncludeAssociations,
            TotalCount = totalCount,
            Items = pagedItems
        };
    }
}