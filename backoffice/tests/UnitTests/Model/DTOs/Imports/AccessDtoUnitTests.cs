using Droits.Models.DTOs.Imports;

namespace Droits.Tests.UnitTests.Model.DTOs.Imports;

public class AccessDtoUnitTests
{
    public AccessDtoUnitTests()
    {
    }

    [Theory]
    [InlineData("18m MHWS", null)]
    [InlineData("5,800m", 5800)]
    [InlineData("3200m", 3200)]
    [InlineData("Max 30m", 30)]
    [InlineData("190ft", null)]
    [InlineData("1m LW", 1)]
    [InlineData("27.2", 27)]
    [InlineData("<8m", 8)]
    [InlineData("5m above datum", 5)]
    [InlineData("Above low tide", null)]
    [InlineData("38m max low water", 38)]
    [InlineData("48-53", 50)]
    [InlineData("30 fathoms", null)]
    [InlineData("3.5 inches", null)]
    [InlineData("<10m", 10)]
    [InlineData("28.1m high neaps", 28)]
    [InlineData("~25m", 25)]
    [InlineData("16 & 13m", null)]
    [InlineData("-10m", 10)]
    [InlineData(">10m", 10)]
    [InlineData("Foreshore", null)]
    [InlineData("5m (at high water)", 5)]
    [InlineData("28m to 30m", 29)]
    [InlineData("Approx 30m", 30)]
    [InlineData("-2 BCD", null)]
    [InlineData("Unknown", null)]
    [InlineData("n/a", null)]
    [InlineData("c. 5m", 5)]
    [InlineData("-22m", 22)]
    [InlineData("20 MSW", null)]
    [InlineData("20MSW", null)]
    [InlineData("object found on wreck next to winch", null)]
    [InlineData("02.22.318 brick 2 to 6: 50.35.300", null)]
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
    public void TestGetDepth(string? depthInput, int? expected)
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