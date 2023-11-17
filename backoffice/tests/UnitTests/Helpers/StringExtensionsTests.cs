using Droits.Helpers.Extensions;

namespace Droits.Tests.UnitTests.Helpers;

public class StringExtensionsTests
{
    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("Hello", true)]
    public void TestHasValue(string? value, bool expected)
    {
        // Act
        var result = value.HasValue();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("Hello", "Hello")]
    public void TestValueOrEmpty(string? value, string expected)
    {
        // Act
        var result = value.ValueOrEmpty();

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
    [InlineData(null, 1, null, "")]
    [InlineData("", 1, null, "")]
    [InlineData("apple", 1, null, "apple")]
    [InlineData("apple", 2, null, "apples")]
    [InlineData("cherry", 2, "cherries", "cherries")]
    [InlineData("city", 2, "cities", "cities")]
    [InlineData("has", 1, "have", "has")]
    [InlineData("has", 2, "have", "have")]
    [InlineData("is", 1, "are", "is")]
    [InlineData("is", 2, "are", "are")]
    [InlineData("sky", -1, null, "skies")]
    public void TestPluralize(string? word, int count, string? pluralForm, string expected)
    {
        // Act
        var result = word!.Pluralize(count, pluralForm);

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
        var exception = Assert.Throws<ArgumentException>(() => dateString.AsDateTime());

        Assert.StartsWith("Invalid date string", exception.Message);
        Assert.Equal(nameof(dateString), exception.ParamName);
    }
    
    [Theory]
    [InlineData("helloWorld", "Hello World")]
    [InlineData("camelCaseString", "Camel Case String")]
    [InlineData("thisIsAProperlyCasedString", "This Is A Properly Cased String")]
    [InlineData("WreckMaterials", "Wreck Materials")]
    [InlineData("", "")]
    [InlineData(null, null)]
    public void ConvertToProperCase_ShouldConvertStringToProperCase(string input, string expected)
    {
        // Act
        var result = input.ConvertToProperCase();

        // Assert
        Assert.Equal(expected, result);
    }
}