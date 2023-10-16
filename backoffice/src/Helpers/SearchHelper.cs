using Droits.Helpers.Extensions;

namespace Droits.Helpers;

public static class SearchHelper
{
    public static bool Matches(string? term, string? value)
    {
        return ( term == null || string.IsNullOrEmpty(term) ||
        value != null  && value.ToLower().Contains(term.ToLower()) );
    }
    
    
    public static bool Matches(bool? term, bool? value)
    {
        return !term.HasValue || value == term;
    }
    
    public static bool Matches(Guid? term, Guid? value)
    {
        return !term.HasValue || value == term;
    }
    
    
    // public static bool IsBetween(DateTime term, DateTime? from, DateTime to)
    // {
    //     return !=term.IsBetween(from, to);
    // }
    //
    //
    // public static bool IsBetween(float? term, float? from, float? to)
    // {
    //     return !term.HasValue || value == term;
    // }
}