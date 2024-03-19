using Droits.Data;
using Droits.Models.Entities;
using Droits.Models.FormModels.SearchFormModels;

namespace Droits.Helpers.SearchHelpers;

public static class WreckQueryBuilder
{
    
    public static IQueryable<Wreck> BuildQuery(WreckSearchForm form, IQueryable<Wreck> query,
        bool usePsql = true)
    {
            
        if ( !string.IsNullOrEmpty(form.WreckName) )
        {
             query = query.Where(w =>
                 !string.IsNullOrEmpty(w.Name) &&
                 (w.Name.ToLower().Contains(form.WreckName.ToLower()) ||
                  (usePsql? CustomEfFunctions.SmallestLevenshteinDistance(form.WreckName.ToLower(), w.Name.ToLower()):
                     SearchHelper.GetLevenshteinDistanceSmallest(form.WreckName.ToLower(), w.Name.ToLower())) <= SearchHelper.GetLevenshteinDistanceThreshold(form.WreckName))
             );
        }

        if ( !string.IsNullOrEmpty(form.OwnerName) )
        {
             query = query.Where(w =>
                 !string.IsNullOrEmpty(w.OwnerName) &&
                 (w.OwnerName.ToLower().Contains(form.OwnerName.ToLower()) ||
                  (usePsql? CustomEfFunctions.SmallestLevenshteinDistance(form.OwnerName.ToLower(), w.OwnerName.ToLower()) :
                      SearchHelper.GetLevenshteinDistanceSmallest(form.OwnerName.ToLower(), w.OwnerName.ToLower())) <= SearchHelper.GetLevenshteinDistanceThreshold(form.OwnerName))
             );
        }
        
        return query;
    }
    
}