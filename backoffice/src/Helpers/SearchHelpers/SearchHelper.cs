
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

    public static int GetLevenshteinDistanceSmallest(string term, string value)
    {
        var totalDistance = GetLevenshteinDistance(term, value);
        var smallestPartDistance = value.Split(" ")
            .Min(word => GetLevenshteinDistance(term, word));
        
        return Math.Min(totalDistance, smallestPartDistance);
    }
    
        
    
    // Calculate Levenshtein distance between two strings, mimicking PostgreSQL's approach.
    public static int GetLevenshteinDistance(string term, string value)
    {
        if (string.IsNullOrEmpty(term))
            return string.IsNullOrEmpty(value) ? 0 : value.Length;
        
        if (string.IsNullOrEmpty(value))
            return term.Length;

        var distance = new int[term.Length + 1, value.Length + 1];

        for (var i = 0; i <= term.Length; i++)
            distance[i, 0] = i;

        for (var j = 0; j <= value.Length; j++)
            distance[0, j] = j;

        for (var i = 1; i <= term.Length; i++)
        for (var j = 1; j <= value.Length; j++)
            distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + (term[i - 1] == value[j - 1] ? 0 : 1));

        return distance[term.Length, value.Length];

    }


    
}