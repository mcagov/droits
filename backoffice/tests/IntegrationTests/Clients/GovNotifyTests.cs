using Droits.Clients;
using Droits.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Droits.Tests;
public class GovNotifyTests
{
    private readonly IGovNotifyClient _client;
    private readonly IConfiguration _configuration;

    public GovNotifyTests(){

        var logger = new Mock<ILogger<GovNotifyClient>>();

        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@"IntegrationTests/Clients/GovNotifyTestData.json", false, false)
            .AddEnvironmentVariables()
            .Build();

        _client = new GovNotifyClient(logger.Object, _configuration);
    }

    [Fact]
    public async void TestSendEmail()
    {
        var form = new EmailForm()
        {
            EmailAddress = "sam.kendell+testing@madetech.com",
            Subject = "Test",
            Body = "This is a test"
        };

        var response = await _client.SendEmailAsync(form);

        Assert.NotNull(response);
    }
}
