using Droits.Helpers.Extensions;
using Droits.Models.Enums;

namespace Droits.Tests.UnitTests.Helpers;

public class StringMapperExtensionsTests
{

    [Theory]
    [InlineData(null, null)]
    [InlineData("", null)]
    [InlineData("123", 123)]
    [InlineData("-456", -456)]
    [InlineData("0", 0)]
    [InlineData("123.45", null)] 
    [InlineData("-789.12", null)]
    [InlineData("abc", null)]
    [InlineData("123abc", null)]
    public void TestAsInt(string? intString, int? expected)
    {
        // Act
        var result = intString.AsInt();

        // Assert
        Assert.Equal(expected, result);
    }
    
    
    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("Yes", true)]
    [InlineData("True", true)]
    [InlineData("No", false)]
    [InlineData("False", false)]
    [InlineData("Yup", true)]
    public void TestAsBoolean(string? value, bool expected)
    {
        // Act
        var result = value.AsBoolean();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("2023-11-15", 2023, 11, 15)] 
    [InlineData("1993-01-05", 1993, 01, 05)] 
    public void AsDateTime_ValidDates(string dateString, int year, int month, int day)
    {
        // Arrange
        var expectedDate = new DateTime(year, month, day);

        // Act
        var actualDate = dateString.AsDateTime();

        // Assert
        Assert.Equal(expectedDate, actualDate);
    }

    [Theory]
    [InlineData("2023-15-11")] 
    [InlineData("")] 
    [InlineData(null)] 
    public void AsDateTime_InvalidDates(string dateString)
    {
        // Act & Assert
        var result = dateString.AsDateTime();
        
        Assert.Null(result);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", null)]
    [InlineData(" ", null)]
    [InlineData("Shipwreck", RecoveredFrom.Shipwreck)]
    [InlineData("Ship Wreck", RecoveredFrom.Shipwreck)]
    [InlineData("seabed", RecoveredFrom.Seabed)]
    [InlineData("AFLOAT", RecoveredFrom.Afloat)]
    [InlineData("Sea Shore", RecoveredFrom.SeaShore)]
    [InlineData("UnknownValue", null)]
    public void AsRecoveredFromEnum_ShouldConvertStringToEnum(string input, RecoveredFrom? expected)
    {
        // Act
        var result = input.AsRecoveredFromEnum();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", null)]
    [InlineData(" ", null)]
    [InlineData("LieuOfSalvage", WreckMaterialOutcome.LieuOfSalvage)]
    [InlineData("Lieu Of Salvage", WreckMaterialOutcome.LieuOfSalvage)]
    [InlineData("RETURNEDTOOWNER", WreckMaterialOutcome.ReturnedToOwner)]
    [InlineData("Donated to Museum", WreckMaterialOutcome.DonatedToMuseum)]
    [InlineData("SoldToMuseum", WreckMaterialOutcome.SoldToMuseum)]
    [InlineData("UnknownValue", WreckMaterialOutcome.Other)]
    public void AsWreckMaterialOutcomeEnum_ShouldConvertStringToEnum(string input, WreckMaterialOutcome? expected)
    {
        // Act
        var result = input.AsWreckMaterialOutcomeEnum();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null, null, null, null, null, null)]
    [InlineData("", "", "", "", "", "")]
    [InlineData("123 Main St\nSpringfield\nIL\nUSA\n12345", "123 Main St", "Springfield", "IL", "USA", "12345")]
    [InlineData("456 Elm St\nShelbyville\nKY\nUSA", "456 Elm St", "Shelbyville", "KY", "USA", "")]
    [InlineData("789 Oak St\nRivertown\nCA", "789 Oak St", "Rivertown", "CA", "", "")]
    [InlineData("SingleLineCSV,Data1,Data2", "SingleLineCSV,Data1,Data2", "", "", "", "")]
    public void AsAddress_ShouldReturnExpectedAddress(string? addressString, string? line1, string? line2, string? town, string? county, string? postcode)
    {
        // Act
        var result = addressString.AsAddress();

        // Assert
        if (string.IsNullOrEmpty(addressString))
        {
            Assert.Null(result);
        }
        else
        {
            Assert.NotNull(result);
            Assert.Equal(line1, result.Line1);
            Assert.Equal(line2, result.Line2);
            Assert.Equal(town, result.Town);
            Assert.Equal(county, result.County);
            Assert.Equal(postcode, result.Postcode);
        }
    }


    [Fact]
    public void AsAddress_ShouldReturnNull_WhenAddressStringIsEmpty()
    {
        // Arrange
        var addressString = string.Empty;

        // Act
        var result = addressString.AsAddress();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void AsAddress_ShouldReturnInLine1_WhenAddressStringIsInvalid()
    {
        // Arrange
        var addressString = "InvalidAddress";

        // Act
        var result = addressString.AsAddress();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(addressString, result.Line1);

    }
}