using System.Globalization;
using Droits.Models.Entities;
using Droits.Models.Enums;

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
    
    public static string RemoveWhitespace(this string input)
    {
        return string.IsNullOrEmpty(input) ? input : new string(input.Where(c => !char.IsWhiteSpace(c)).ToArray());
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

    public static string ConvertToProperCase(this string input) =>
        string.IsNullOrEmpty(input) ? input : char.ToUpper(input[0]) +
                                              string.Concat(input.Skip(1).Select((x, i) => char.IsUpper(x) ? " " + x : x.ToString()));

}