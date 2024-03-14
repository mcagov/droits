#region
using  FuzzyString;

#endregion

namespace Droits.Helpers.SearchHelpers;

public static class SearchHelper
{
    private const int MaxLevenshteinDistance = 5;
    private const int MidLevenshteinDistance = 3;
    private const int LowerLevenshteinDistance = 1;
    public static int GetLevenshteinDistanceThreshold(string? query)
    {
        if ( string.IsNullOrEmpty(query) )
        {
            return 0;
        }

        return query.Length switch
        {
            <= 3 => 0,
            <= 5 => LowerLevenshteinDistance,
            <= 10 => MidLevenshteinDistance,
            _ => MaxLevenshteinDistance
        };
    }

    public static int GetLevenshteinDistance(string term, string value) => int.Max(0,term.LevenshteinDistance(value));

}