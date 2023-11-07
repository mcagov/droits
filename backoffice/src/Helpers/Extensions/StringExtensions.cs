namespace Droits.Helpers.Extensions;

public static class StringExtensions
{
    public static bool HasValue(this string? value)
    {
        return !string.IsNullOrEmpty(value);
    }


    public static string ValueOrEmpty(this string? value)
    {
        if ( value != null && value.HasValue() )
        {
            return value;
        }

        return string.Empty;
    }
}