using Droits.Clients;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
namespace Droits.IntegrationTests.Clients;
public class GovNotifyTests : IClassFixture<TestFixture>
{
    private readonly IGovNotifyClient _client;
    private readonly IConfiguration _configuration;

    public GovNotifyTests(TestFixture fixture){

        var logger = new Mock<ILogger<GovNotifyClient>>();

        _configuration = fixture.Configuration;

        _client = new GovNotifyClient(logger.Object, _configuration);
    }

    [Fact]
    public async void SendLetterAsync_ShouldReturnAValidResponse()
    {
        var model = new Letter()
        {
            Recipient = "sam.kendell+testing@madetech.com",
            Subject = "Test",
            Body = "This is a test"
        };

        var response = await _client.SendLetterAsync(model);

        Assert.NotNull(response);
    }
    [Fact]
    public async void SendLetterAsync_ShouldReturnSubmittedTextInTheBody()
    {
        var model = new Letter()
        {
            Recipient = "sam.kendell+testing@madetech.com",
            Subject = "Test",
            Body = "This is a test"
        };

        var response = await _client.SendLetterAsync(model);
        Assert.Equal("This is a test",response.content.body);
    }
    [Fact]
    public async void SendLetterAsync_ShouldReturnSubmittedSubjectInTheSubject()
    {
        var model = new Letter()
        {
            Recipient = "sam.kendell+testing@madetech.com",
            Subject = "Test",
            Body = "This is a test"
        };

        var response = await _client.SendLetterAsync(model);

        Assert.Equal("Test",response.content.subject);
    }
}
