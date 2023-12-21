using Droits.Helpers;
using Droits.Helpers.SearchHelpers;

namespace Droits.Tests.UnitTests.Helpers;

public class SearchHelperTests
{
    
    [Theory]
    [InlineData("kitten", "sitting", 3)] // Example from Levenshtein distance algorithm
    [InlineData("example", "example", 0)] // Same strings, distance should be 0
    [InlineData("hello", "world", 4)]    // Different strings, distance should be 4
    [InlineData("", "somevalue", 9)]      // One string is empty, distance should be length of the non-empty string
    [InlineData("abc", "", 3)]            // One string is empty, distance should be length of the non-empty string
    [InlineData("", "", 0)]               // Both strings are empty, distance should be 0
    public void LevenshteinDistance_ReturnsCorrectDistance(string term, string value, int expectedDistance)
    {
        // Arrange - Already arranged via InlineData attribute

        // Act
        var distance = SearchHelper.GetLevenshteinDistance(term, value);

        // Assert
        Assert.Equal(expectedDistance, distance);
    }
}