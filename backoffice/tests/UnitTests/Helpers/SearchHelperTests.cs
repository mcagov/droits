using Droits.Helpers;
using Droits.Helpers.SearchHelpers;

namespace Droits.Tests.UnitTests.Helpers;

public class SearchHelperTests
{
    
    [Theory]
    [InlineData("kitten", "sitting", 3)]
    [InlineData("example", "example", 0)]
    [InlineData("hello", "world", 4)]
    [InlineData("", "somevalue", 9)]
    [InlineData("abc", "", 3)]
    [InlineData("", "", 0)]
    [InlineData("apple banana cherry", "banana", 13)]
    [InlineData("apple banana cherry", "apple mango cherry", 4)]
    public void GetLevenshteinDistance_ReturnsCorrectDistance(string term, string value, int expectedDistance)
    {
        var distance = SearchHelper.GetLevenshteinDistance(term, value);
        Assert.Equal(expectedDistance, distance);
    }

}