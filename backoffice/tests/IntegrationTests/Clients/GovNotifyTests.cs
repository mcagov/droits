using Droits.Clients;
using Droits.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Droits.Tests;
public class GovNotifyTests
{
    private readonly IGovNotifyClient _client;

    public GovNotifyTests(){

        var logger = new Mock<ILogger<GovNotifyClient>>();
        var config = new Mock<IConfiguration>();

        var apiKey = ""; //TODO - pull from config.
        var templateId = ""; //TODO - pull from config.

        config.SetupGet(x => x.GetSection("GovNotify:ApiKey").Value).Returns(apiKey);
        config.SetupGet(x => x.GetSection("GovNotify:TemplateId").Value).Returns(templateId);

        _client = new GovNotifyClient(logger.Object, config.Object);
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
