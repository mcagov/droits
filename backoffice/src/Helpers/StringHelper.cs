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
    
    public static string FormatUrl(string url)
    {
        if ( string.IsNullOrWhiteSpace(url) ) return url;
        if (!url.StartsWith("http://") && !url.StartsWith("https://"))
        {
            url = "https://" + url;
        }
        return url;
    }

    public static string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        var firstChar = input.Substring(0, 1).ToUpper();
        return string.Concat(firstChar, input.AsSpan(1, input.Length - 1));
    }
}