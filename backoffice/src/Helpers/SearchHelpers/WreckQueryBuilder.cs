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
            
        if ( !string.IsNullOrEmpty(form.Name) )
        {
             query = query.Where(w =>
                 !string.IsNullOrEmpty(w.Name) &&
                 (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.Name.ToLower(), w.Name.ToLower()) : 
                     SearchHelper.GetLevenshteinDistance(form.Name.ToLower(), w.Name.ToLower())) < MaxLevenshteinDistance ||
                 w.Name.ToLower().Contains(form.Name.ToLower())
             );
        }
        
        return query;
    }
    
}