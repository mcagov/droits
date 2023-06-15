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
            .AddJsonFile(@"appsettings.json", false, false)
            .AddEnvironmentVariables()
            .Build();

        _client = new GovNotifyClient(logger.Object, _configuration);
    }

    [Fact]
    public async void SendEmailAsync_ShouldReturnAValidResponse()
    {
        var form = new EmailForm()
        {
            Recipient = "sam.kendell+testing@madetech.com",
            Subject = "Test",
            Body = "This is a test"
        };

        var response = await _client.SendEmailAsync(form);

        Assert.NotNull(response);
    }
    [Fact]
    public async void SendEmailAsync_ShouldReturnSubmittedTextInTheBody()
    {
        var form = new EmailForm()
        {
            Recipient = "sam.kendell+testing@madetech.com",
            Subject = "Test",
            Body = "This is a test"
        };

        var response = await _client.SendEmailAsync(form);

        Assert.Equal("This is a test",response.content.body);
    }
    [Fact]
    public async void SendEmailAsync_ShouldReturnSubmittedSubjectInTheSubject()
    {
        var form = new EmailForm()
        {
            Recipient = "sam.kendell+testing@madetech.com",
            Subject = "Test",
            Body = "This is a test"
        };

        var response = await _client.SendEmailAsync(form);

        Assert.Equal("Test",response.content.subject);
    }
}
