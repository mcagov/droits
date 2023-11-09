#region

using Droits.Models.ViewModels.ListViews;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Droits.Helpers;

public static class ServiceHelper
{
    public static async Task<ListView<TView>> GetPagedResult<TView>(
        IQueryable<TView> query,
        SearchOptions searchOptions) where TView : class
    {
        var totalCount = await query.CountAsync();

        var totalPageCount = (int)Math.Ceiling((double)totalCount / searchOptions.PageSize);

        if (searchOptions.PageNumber > totalPageCount || searchOptions.PageNumber <= 0)
        {
            searchOptions.PageNumber = 1;
        }
        
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