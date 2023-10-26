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
        return !term.HasValue || value == term ||
               ( term == default(Guid) && value == null );
    }
    
    public static bool IsBetween(DateTime? value, DateTime? from, DateTime? to)
    {
        if ( from == null && to == null )
        {
            return true;
        }
        
        return value.HasValue && value.Value.IsBetween(from, to);
    }
    
    
    public static bool IsBetween(float? value, float? from, float? to)
    {
        if ( from == null && to == null )
        {
            return true;
        }
        
        return value.HasValue && ( value.Value >= from || from == null ) && ( value.Value <= to || to == null );
    }
}