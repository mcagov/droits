using Droits.Helpers;

namespace Droits.Tests.UnitTests.Helpers;

public class SearchHelperTests
{
    [Fact]
    public void Matches_WithTwoIdenticalStrings_ShouldReturnTrue()
    {
        Assert.True(SearchHelper.Matches("foo","foo"));
    }
    
    [Fact]
    public void Matches_WithStringToMatchContainedInAString_ShouldReturnTrue()
    {
        Assert.True(SearchHelper.Matches("foo","foobar"));
    }
    [Fact]
    public void Matches_WithStringToMatchNotContainedInAString_ShouldReturnFalse()
    {
        Assert.False(SearchHelper.Matches("foo","baz"));
    }
    [Fact]
    public void Matches_WithAnEmptyStringAndAString_ShouldReturnTrue()
    {
        Assert.True(SearchHelper.Matches("","foo"));
    }
    [Fact]
    public void Matches_WithAStringAndANullString_ShouldReturnFalse()
    {
        Assert.False(SearchHelper.Matches("foo",null));
    }
    [Fact]
    public void Matches_WithNullAndAString_ShouldReturnTrue()
    {
        Assert.True(SearchHelper.Matches(null,"foo"));
    }
     [Fact]
    public void Matches_WithAnEmptyStringAndNull_ShouldReturnTrue()
    {
        Assert.True(SearchHelper.Matches("",null));
    }
    [Fact]
    public void Matches_WithIdenticalBooleans_ShouldReturnTrue()
    {
        Assert.True(SearchHelper.Matches(true,true));
    }
    [Fact]
    public void Matches_WithDifferentBooleans_ShouldReturnFalse()
    {
        Assert.False(SearchHelper.Matches(true,false));
    }
    [Fact]
    public void Matches_WithANullAndBoolean_ShouldReturnTrue()
    {
        Assert.True(SearchHelper.Matches(null,true));
    }
    [Fact]
    public void IsBetween_WithValueBetweenTwoDoubles_ShouldReturnTrue()
    {
        Assert.True(SearchHelper.IsBetween(0.2d,0.1d,0.3d));
    }
    [Fact]
    public void IsBetween_WithValueNotBetweenTwoDoubles_ShouldReturnFalse()
    {
        Assert.False(SearchHelper.IsBetween(0.5d,0.1d,0.3d));
    }
    [Fact]
    public void IsBetween_WithValueBetweenADoubleAndNull_ShouldReturnTrue()
    {
        Assert.True(SearchHelper.IsBetween(0.2d,0.1d,null));
        Assert.True(SearchHelper.IsBetween(0.2,null,0.3d));
    }
    [Fact]
    public void IsBetween_WithTwoNulls_ShouldReturnTrue()
    {
        Assert.True(SearchHelper.IsBetween(0.2d,null,null));
    }
    [Fact]
    public void IsBetween_WithADateBetweenTwoDates_ShouldReturnTrue()
    {
        Assert.True(SearchHelper.IsBetween(new DateTime(2023,10,25),new DateTime(2023,10,24),
            new DateTime(2023,10,26)));
    }
    [Fact]
    public void IsBetween_WithADateBetweenNullAndADate_ShouldReturnTrue()
    {
        Assert.True(SearchHelper.IsBetween(new DateTime(2023,10,25),null,
            new DateTime(2023,10,26)));
    }
    [Fact]
    public void IsBetween_WithADateBetweenADateAndNull_ShouldReturnTrue()
    {
        Assert.True(SearchHelper.IsBetween(new DateTime(2023,10,25),new DateTime(2023,10,24),
            null));
    }
    [Fact]
    public void IsBetween_WithADateAndTwoNulls_ShouldReturnTrue()
    {
        Assert.True(SearchHelper.IsBetween(new DateTime(2023,10,25),null, null));
    }
    [Fact]
    public void IsBetween_WithNullAndTwoDates_ShouldReturnFalse()
    {
        Assert.False(SearchHelper.IsBetween(null,new DateTime(2023,10,25), new DateTime(2023,10,26)));
    }
    [Fact]
    public void IsBetween_WithAllDateTimeNulls_ShouldReturnTrue()
    {
        Assert.True(SearchHelper.IsBetween((DateTime?)null,null, null));
    }
    [Fact]
    public void IsBetween_WithAllDoubleNulls_ShouldReturnTrue()
    {
        Assert.True(SearchHelper.IsBetween((double?)null,null, null));
    }
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