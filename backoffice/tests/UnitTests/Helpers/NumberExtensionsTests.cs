using Droits.Helpers.Extensions;

namespace Droits.Tests.UnitTests.Helpers;

public class NumberExtensionsTests
{
    
    [Theory]
    [InlineData(null, null)]
    [InlineData(123.0d, 123)]
    [InlineData(-456.0d, -456)]
    [InlineData(0.0d, 0)]
    [InlineData(123.49d, 123)]
    [InlineData(123.5d, 124)]
    [InlineData(123.51d, 124)]
    [InlineData(-789d, -789)]
    [InlineData(double.MinValue, null)] 
    [InlineData(double.MaxValue, null)]
    public void TestAsInt(double? doubleNumber, int? expected)
    {
        // Act
        var result = doubleNumber.AsInt();

        // Assert
        Assert.Equal(expected, result);
    }
}