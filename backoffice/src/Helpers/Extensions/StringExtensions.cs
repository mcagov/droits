namespace Droits.Helpers.Extensions;

public static class StringExtensions
{
    public static bool HasValue(this string? value)
    {
        return !string.IsNullOrEmpty(value);
    }


    public static string? ValueOrEmpty(this string? value)
    {
        return string.IsNullOrEmpty(value) ? string.Empty : value;
    }


    public static bool AsBoolean(this string? value)
    {
        if ( string.IsNullOrEmpty(value) )
        {
            return false;
        }

        value = value.ToLower().Trim();
        
        return value.StartsWith("y") || value.StartsWith("t");
    }
    
    public static string Pluralize(this string? word, int count, string? pluralForm = null)
    {
        if ( string.IsNullOrEmpty(word) )
        {
            return string.Empty;
        }
        return count == 1
            ? word
            : pluralForm ?? (word.EndsWith("y", StringComparison.OrdinalIgnoreCase) ? string.Concat(word.AsSpan(0, word.Length - 1), "ies") : word + "s");
    }
}