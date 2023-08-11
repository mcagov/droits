using Microsoft.Extensions.Configuration;
using Droits.Tests;
using Droits.Tests.Helpers;

namespace Droits.Tests.UnitTests;
public class TestFixtureUnitTests : IClassFixture<TestFixture>
{

    public IConfiguration _configuration {get;}

    public TestFixtureUnitTests(TestFixture fixture)
    {
        _configuration = fixture.Configuration;
    }

    [Fact]
    public void TestConfiguration_GetConfiguration(){

        //when
        var testConfigValue = _configuration.GetSection("TestValue").Value;

        Assert.Equal("test",testConfigValue);
    }

}

