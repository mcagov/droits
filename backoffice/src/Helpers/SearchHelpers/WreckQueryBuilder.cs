using Droits.Helpers.Extensions;
using Droits.Models.Entities;
using Droits.Models.FormModels.SearchFormModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Droits.Helpers.SearchHelpers;

public static class WreckQueryBuilder
{
    
    private const int MaxLevenshteinDistance = 5;
    
    public static IQueryable<Wreck> BuildQuery(WreckSearchForm form, IQueryable<Wreck> query,
        bool usePsql = true)
    {
            
        if ( !string.IsNullOrEmpty(form.WreckName) )
        {
             query = query.Where(w =>
                 !string.IsNullOrEmpty(w.Name) &&
                 (w.Name.ToLower().Contains(form.WreckName.ToLower()) ||
                  (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.WreckName.ToLower(), w.Name.ToLower()) :
                      SearchHelper.GetLevenshteinDistance(form.WreckName.ToLower(), w.Name.ToLower())) < MaxLevenshteinDistance)
             );
        }
        
        if ( !string.IsNullOrEmpty(form.OwnerName) )
        {
             query = query.Where(w =>
                 !string.IsNullOrEmpty(w.OwnerName) &&
                 (w.OwnerName.ToLower().Contains(form.OwnerName.ToLower()) ||
                  (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.OwnerName.ToLower(), w.OwnerName.ToLower()) :
                      SearchHelper.GetLevenshteinDistance(form.OwnerName.ToLower(), w.OwnerName.ToLower())) < MaxLevenshteinDistance)
             );
        }
        
        return query;
    }
    
}