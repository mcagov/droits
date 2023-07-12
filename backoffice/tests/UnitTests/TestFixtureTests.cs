using Microsoft.Extensions.Configuration;

namespace Droits.Tests.UnitTests;

public class TestFixtureUnitTests : IClassFixture<TestFixture>
{
    public TestFixtureUnitTests(TestFixture fixture)
    {
        _configuration = fixture.Configuration;
    }

    public IConfiguration _configuration { get; }

    [Fact]
    public void TestConfiguration_GetConfiguration()
    {
        //when
        var testConfigValue = _configuration.GetSection("TestValue").Value;

        Assert.Equal("test", testConfigValue);
    }
}