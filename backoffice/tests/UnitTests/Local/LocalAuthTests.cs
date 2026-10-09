using System.Security.Claims;
using Droits.Local;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace Droits.Tests.UnitTests.Local;

public class LocalAuthTests
{
    private static IConfiguration Configuration(Dictionary<string, string?> values) =>
        new ConfigurationBuilder().AddInMemoryCollection(values).Build();

    private static IHostEnvironment Environment(string name)
    {
        var environment = new Mock<IHostEnvironment>();
        environment.Setup(e => e.EnvironmentName).Returns(name);
        return environment.Object;
    }

    [Fact]
    public void IsEnabled_WhenRequestedInDevelopment_ReturnsTrue()
    {
        var configuration = Configuration(new() { [LocalAuth.Flag] = "true" });

        Assert.True(LocalAuth.IsEnabled(configuration, Environment(Environments.Development)));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("false")]
    [InlineData("yes")]
    public void IsEnabled_WhenNotRequested_ReturnsFalse(string? flag)
    {
        var configuration = Configuration(new() { [LocalAuth.Flag] = flag });

        Assert.False(LocalAuth.IsEnabled(configuration, Environment(Environments.Development)));
    }

    [Theory]
    [InlineData("Production")]
    [InlineData("Staging")]
    [InlineData("")]
    public void IsEnabled_OutsideDevelopment_ReturnsFalse(string environmentName)
    {
        var configuration = Configuration(new() { [LocalAuth.Flag] = "true" });

        Assert.True(LocalAuth.IsRequested(configuration));
        Assert.False(LocalAuth.IsEnabled(configuration, Environment(environmentName)));
    }

    [Theory]
    [InlineData("ECS_CONTAINER_METADATA_URI_V4")]
    [InlineData("ECS_CONTAINER_METADATA_URI")]
    [InlineData("ECS_ENABLE_CONTAINER_METADATA")]
    public void IsEnabled_WhenRunningInEcs_ReturnsFalse(string marker)
    {
        var configuration = Configuration(new() { [LocalAuth.Flag] = "true", [marker] = "true" });

        Assert.False(LocalAuth.IsEnabled(configuration, Environment(Environments.Development)));
    }

    [Fact]
    public void Claims_UseDefaults_WhenNothingIsConfigured()
    {
        var claims = LocalAuth.Claims(Configuration(new())).ToList();

        Assert.Equal("00000000-0000-4000-8000-000000000001", claims.Single(c => c.Type == LocalAuth.ObjectIdClaimType).Value);
        Assert.Equal("Dev User", claims.Single(c => c.Type == "name").Value);
        Assert.Equal("dev@droits.local", claims.Single(c => c.Type == "preferred_username").Value);
        Assert.Equal(string.Empty, claims.Single(c => c.Type == "groups").Value);
    }

    [Fact]
    public void Claims_UseConfiguredUserAndAzureAdGroup()
    {
        var configuration = Configuration(new()
        {
            ["LOCAL_AUTH_EMAIL"] = "someone@droits.local",
            ["LOCAL_AUTH_NAME"] = "Someone",
            ["AzureAd:GroupId"] = "group-id"
        });

        var claims = LocalAuth.Claims(configuration).ToList();

        Assert.Equal("someone@droits.local", claims.Single(c => c.Type == "preferred_username").Value);
        Assert.Equal("Someone", claims.Single(c => c.Type == "name").Value);
        Assert.Equal("group-id", claims.Single(c => c.Type == "groups").Value);
    }

    [Fact]
    public async Task Handler_AuthenticatesEveryRequestAsTheLocalUser()
    {
        var options = new Mock<IOptionsMonitor<AuthenticationSchemeOptions>>();
        options.Setup(o => o.Get(It.IsAny<string>())).Returns(new AuthenticationSchemeOptions());
        var handler = new LocalAuthenticationHandler(options.Object, NullLoggerFactory.Instance, UrlEncoder.Default, Configuration(new()));

        await handler.InitializeAsync(
            new AuthenticationScheme(LocalAuth.Scheme, null, typeof(LocalAuthenticationHandler)),
            new DefaultHttpContext());
        var result = await handler.AuthenticateAsync();

        Assert.True(result.Succeeded);
        Assert.True(result.Principal!.Identity!.IsAuthenticated);
        Assert.Equal("dev@droits.local", result.Principal.FindFirstValue("preferred_username"));
    }
}
