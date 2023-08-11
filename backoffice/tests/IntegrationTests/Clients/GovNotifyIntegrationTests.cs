using Droits.Clients;
using Droits.Models.Entities;
using Droits.Tests.Helpers;
using Microsoft.Extensions.Logging;

namespace Droits.Tests.IntegrationTests.Clients;
public class GovNotifyIntegrationTests : IClassFixture<TestFixture>
{
    private readonly IGovNotifyClient _client;


    public GovNotifyIntegrationTests(TestFixture fixture){
        var logger = new Mock<ILogger<GovNotifyClient>>();

        var configuration = fixture.Configuration;

        _client = new GovNotifyClient(logger.Object, configuration);
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
