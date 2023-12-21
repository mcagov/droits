#region
using DuoVia.FuzzyStrings;

#endregion

namespace Droits.Helpers.SearchHelpers;

public static class SearchHelper
{

    public static int GetLevenshteinDistance(string term, string value) => term.LevenshteinDistance(value);

}