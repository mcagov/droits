using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using Droits.Helpers;

namespace Droits.Tests.UnitTests.Helpers;

public class ConstantsTests
{
    [Fact]
    public void PostcodeMatch_WithPostcodeWithSpace_ShouldReturnTrue()
    {
        var postcode = "BI9 3LF";
        var regex = new Regex(Constants.PostcodeRegex);
        Assert.True(regex.Match(postcode).Success);
    }
    [Fact]
    public void PostcodeMatch_WithPostcodeWithoutSpace_ShouldReturnTrue()
    {
        var postcode = "BI93LF";
        var regex = new Regex(Constants.PostcodeRegex);
        Assert.True(regex.Match(postcode).Success);
    }
    [Fact]
    public void PostcodeMatch_WithPostcodeWithFourthLetterInGroupOne_ShouldReturnTrue()
    {
        var postcode = "SW1A 1AA";
        var regex = new Regex(Constants.PostcodeRegex);
        Assert.True(regex.Match(postcode).Success);
    }
    [Fact]
    public void PostcodeMatch_WithNotAPostcode_ShouldReturnFalse()
    {
        var postcode = "no good";
        var regex = new Regex(Constants.PostcodeRegex);
        Assert.False(regex.Match(postcode).Success);
    }
}