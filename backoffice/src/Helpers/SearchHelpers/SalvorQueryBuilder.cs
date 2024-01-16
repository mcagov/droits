using Droits.Models.Entities;
using Droits.Models.FormModels.SearchFormModels;
using Microsoft.EntityFrameworkCore;

namespace Droits.Helpers.SearchHelpers;

public class SalvorQueryBuilder
{
    private const int MaxLevenshteinDistance = 5;
    
    public static IQueryable<Salvor> BuildQuery(SalvorSearchForm form, IQueryable<Salvor> query,
        bool usePsql = false) // change to true when using real db
    { 
        if ( !string.IsNullOrEmpty(form.Name) )
        {
             query = query.Where(s =>
                 !string.IsNullOrEmpty(s.Name) &&
                 (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.Name.ToLower(), s.Name.ToLower()) : 
                     SearchHelper.GetLevenshteinDistance(form.Name.ToLower(), s.Name.ToLower())) < MaxLevenshteinDistance ||
                 s.Name.ToLower().Contains(form.Name.ToLower())
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