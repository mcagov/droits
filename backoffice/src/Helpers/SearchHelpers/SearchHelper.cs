#region
using  FuzzyString;

#endregion

namespace Droits.Helpers.SearchHelpers;

public static class SearchHelper
{

    public static int GetLevenshteinDistance(string term, string value) => int.Max(0,term.LevenshteinDistance(value));

}