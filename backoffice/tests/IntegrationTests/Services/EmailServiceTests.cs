using Droits.Clients;
using Droits.Services;
using Droits.Models;
using Droits.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Droits.Tests.IntegrationTests.Services;

public class EmailServiceTests
{
    private readonly IEmailService _service;

    public EmailServiceTests()
    {
        var logger = new Mock<ILogger<EmailService>>();
        Mock<IEmailRepository> mockEmailRepository = new Mock<IEmailRepository>();

        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@"appsettings.Development.json", false, false)
            .AddEnvironmentVariables()
            .Build();
        
        var client = new GovNotifyClient(new Mock<ILogger<GovNotifyClient>>().Object, config);

        _service = new EmailService(logger.Object, client, mockEmailRepository.Object);
    }
    
    [Fact]
    public async void SendEmailAsync_ShouldReturnAValidResponse()
    {
        var form = new EmailForm()
        {
            EmailAddress = "sam.kendell+testing@madetech.com",
            Subject = "Test",
            Body = "This is a test"
        };

        var response = await _service.SendEmailAsync(form);

        Assert.NotNull(response);
    }
    [Fact]
    public async void SendEmailAsync_ShouldReturnSubmittedTextInTheBody()
    {
        var form = new EmailForm()
        {
            EmailAddress = "sam.kendell+testing@madetech.com",
            Subject = "Test",
            Body = "This is a test"
        };

        var response = await _service.SendEmailAsync(form);

        Assert.Equal("This is a test",response.content.body);
    }
    [Fact]
    public async void SendEmailAsync_ShouldReturnSubmittedSubjectInTheSubject()
    {
        var form = new EmailForm()
        {
            EmailAddress = "sam.kendell+testing@madetech.com",
            Subject = "Test",
            Body = "This is a test"
        };
        
        

        var response = await _service.SendEmailAsync(form);

        Assert.Equal("Test",response.content.subject);
    }
}