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
}