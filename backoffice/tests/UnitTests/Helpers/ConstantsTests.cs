using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using Droits.Helpers;

namespace Droits.Tests.UnitTests.Helpers;

public class ConstantsTests
{
    private readonly Regex regex;

    public ConstantsTests()
    {
        regex = new Regex(Constants.PostcodeRegex);
    }
    
    [Fact]
    public void PostcodeMatch_WithPostcodeWithSpace_ShouldReturnTrue()
    {
        var postcode = "BI9 3LF";
        Assert.True(regex.Match(postcode).Success);
    }
    [Fact]
    public void PostcodeMatch_WithPostcodeWithoutSpace_ShouldReturnTrue()
    {
        var postcode = "BI93LF";
        Assert.True(regex.Match(postcode).Success);
    }
    [Fact]
    public void PostcodeMatch_WithPostcodeWithFourthLetterInGroupOne_ShouldReturnTrue()
    {
        var postcode = "SW1A 1AA";
        Assert.True(regex.Match(postcode).Success);
    }
    [Fact]
    public void PostcodeMatch_WithNotAPostcode_ShouldReturnFalse()
    {
        var postcode = "no good";
        Assert.False(regex.Match(postcode).Success);
    }
}