namespace Droits.Helpers;

public static class StringHelper
{
    public static string JoinWithSeparator(string? separator, params string?[] values)
    {

        if ( string.IsNullOrEmpty(separator) )
        {
            separator = ", ";
        }
        return string.Join(separator, values.Where(s => !string.IsNullOrEmpty(s)));
    }
}