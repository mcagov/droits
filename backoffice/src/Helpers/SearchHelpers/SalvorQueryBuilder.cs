using Droits.Data;
using Droits.Models.Entities;
using Droits.Models.FormModels.SearchFormModels;

namespace Droits.Helpers.SearchHelpers;

public class SalvorQueryBuilder
{
    
    public static IQueryable<Salvor> BuildQuery(SalvorSearchForm form, IQueryable<Salvor> query,
        bool usePsql = true)
    { 
        if ( !string.IsNullOrEmpty(form.Name) )
        {
             query = query.Where(s =>
                 !string.IsNullOrEmpty(s.Name) &&
                 (s.Name.ToLower().Contains(form.Name.ToLower()) ||
                  (usePsql? CustomEfFunctions.SmallestLevenshteinDistance(form.Name.ToLower(), s.Name.ToLower()) :
                      SearchHelper.GetLevenshteinDistanceSmallest(form.Name.ToLower(), s.Name.ToLower())) <= SearchHelper.GetLevenshteinDistanceThreshold(form.Name))
             );
        }
        
        if ( !string.IsNullOrEmpty(form.Email) )
        {
            query = query.Where(s =>
                 !string.IsNullOrEmpty(s.Email) &&
                 s.Email.ToLower().Contains(form.Email.ToLower())
             );
        }
        
        return query;
    }
}