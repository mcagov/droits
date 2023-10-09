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
    public void Matches_WithStringToMatchContainedInSomething_ShouldReturnTrue()
    {
        Assert.True(SearchHelper.Matches("foo","foobar"));
    }
    [Fact]
    public void Matches_WithStringToMatchNotContainedInSomething_ShouldReturnFalse()
    {
        Assert.False(SearchHelper.Matches("foo","baz"));
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
}