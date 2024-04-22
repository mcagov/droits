using Droits.Models.DTOs.Exports;
using Droits.Models.DTOs.Imports;
using Droits.Models.Entities;

namespace Droits.Tests.UnitTests.Model.DTOs.Imports;

public class AccessDtoUnitTests
{
    public AccessDtoUnitTests()
    {
        
    }
    
    [Theory]
    [InlineData(null, null)]
    [InlineData("40-50m", 45)]
    [InlineData("52.4", 52)]
    [InlineData("53m", 53)]
    [InlineData("36m", 36)]
    [InlineData("40m-50m", 45)]
    [InlineData("4 0 -50m", 45)]
    [InlineData("test", null)]
    [InlineData("40metres-unknown", null)]
    [InlineData("quite deep", null)]
    [InlineData("10 (assume m)",10)]
    public void TestAsInt(string? depthInput, int? expected)
    {
        var dto = new AccessDto()
        {
            Depth = depthInput
        };
        
        // Act
        var result = dto.GetDepth();

        // Assert
        Assert.Equal(expected, result);
    }

}