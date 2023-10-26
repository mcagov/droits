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
    public void IsBetween_WithValueBetweenTwoFloats_ShouldReturnTrue()
    {
        Assert.True(SearchHelper.IsBetween((float)0.2,(float)0.1,(float)0.3));
    }
    [Fact]
    public void IsBetween_WithValueNotBetweenTwoFloats_ShouldReturnFalse()
    {
        Assert.False(SearchHelper.IsBetween((float)0.5,(float)0.1,(float)0.3));
    }
    [Fact]
    public void IsBetween_WithValueBetweenAFloatAndNull_ShouldReturnTrue()
    {
        Assert.True(SearchHelper.IsBetween((float)0.2,(float)0.1,null));
        Assert.True(SearchHelper.IsBetween((float)0.2,null,(float)0.3));
    }
    [Fact]
    public void IsBetween_WithTwoNulls_ShouldReturnTrue()
    {
        Assert.True(SearchHelper.IsBetween((float)0.2,null,null));
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
    public void IsBetween_WithAllFloatNulls_ShouldReturnTrue()
    {
        Assert.True(SearchHelper.IsBetween((float?)null,null, null));
    }
}