using Droits.Helpers;

namespace Droits.Tests.UnitTests.Helpers;

public class StringHelperUnitTests
{
    // StringHelper.CapitalizeFirstLetter
    
    [Fact]
    public void ShouldCapitalizeFirstLetter()
    {
        var response = StringHelper.CapitalizeFirstLetter("this should be capitalized.");
        
        Assert.Equal("This should be capitalized.", response);
    }
    
    [Fact]
    public void ShouldReturnNullWithNullInput()
    {
        var response = StringHelper.CapitalizeFirstLetter(null);
        
        Assert.Null(response);
    }
    
    [Fact]
    public void ShouldReturnEmptyStringWithEmptyStringInput()
    {
        var response = StringHelper.CapitalizeFirstLetter("");
        
        Assert.Equal("", response);
    }
}