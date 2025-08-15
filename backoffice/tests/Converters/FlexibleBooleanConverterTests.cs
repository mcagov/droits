using System.Text.Json;
using Droits.Converters;

namespace Droits.Tests.Converters;

public class FlexibleBooleanConverterTests
{
    private readonly JsonSerializerOptions _options = new()
    {
        Converters =
        {
            new FlexibleBooleanConverter(),
            new FlexibleNullableBooleanConverter()
        }
    };


    [Theory]
    [InlineData("""
                "true"
                """, true)]
    [InlineData("""
                "false"
                """, false)]
    [InlineData("""
                "True"
                """, true)]
    [InlineData("""
                "False"
                """, false)]
    [InlineData("""
                "   "
                """, false)]
    [InlineData("""
                ""
                """, false)]
    [InlineData("true", true)]
    [InlineData("false", false)]
    public void CanDeserialize_Bool(string json, bool expected)
    {
        var result = JsonSerializer.Deserialize<bool>(json, _options);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("""
                "true"
                """, true)]
    [InlineData("""
                "false"
                """, false)]
    [InlineData("""
                "   "
                """, null)]
    [InlineData("""
                ""
                """, null)]
    [InlineData("true", true)]
    [InlineData("false", false)]
    public void CanDeserialize_NullableBool(string json, bool? expected)
    {
        var result = JsonSerializer.Deserialize<bool?>(json, _options);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void CanSerialize_Bool_True()
    {
        var json = JsonSerializer.Serialize(true, _options);
        Assert.Equal("true", json);
    }

    [Fact]
    public void CanSerialize_NullableBool_Null()
    {
        var json = JsonSerializer.Serialize<bool?>(null, _options);
        Assert.Equal("null", json);
    }
}