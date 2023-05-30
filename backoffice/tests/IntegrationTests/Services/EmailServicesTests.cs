using Droits.Clients;
using Droits.Services;
using Droits.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Droits.Tests;

public class EmailServicesTests
{
    private readonly IEmailService _service;

    public EmailServicesTests()
    {
        var logger = new Mock<ILogger<EmailService>>();
        
        
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@"appsettings.json", false, false)
            .AddEnvironmentVariables()
            .Build();
        
        var client = new GovNotifyClient(new Mock<ILogger<GovNotifyClient>>().Object, config);

        _service = new EmailService(logger.Object, client);
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

        var response = await _service.SendEmailAsync(form);

        Assert.NotNull(response);
    }
    [Fact]
    public async void TestSendEmail2()
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
    public async void TestSendEmail3()
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