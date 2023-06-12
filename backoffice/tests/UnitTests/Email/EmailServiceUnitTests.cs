using Droits.Clients;
using Droits.Services;
using Droits.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace Droits.Tests.UnitTests.Email;

public class EmailServiceUnitTests
{
    private readonly IEmailService _service;

    public EmailServiceUnitTests()
    {
        Mock<ILogger<EmailService>> mockLogger = new Mock<ILogger<EmailService>>();
        Mock<IGovNotifyClient> mockClient = new Mock<IGovNotifyClient>();
        Mock<IEmailRepository> mockEmailRepository = new Mock<IEmailRepository>();

        _service = new EmailService(
            mockLogger.Object, 
            mockClient.Object,
            mockEmailRepository.Object);
    }
    
    [Fact]
    public void GetTemplateFilename_WhenTheEmailTemplateTypeIsReportAcknowledged_ShouldReturnReportAcknowledged()
    {
        string templateFilename = _service.GetTemplateFilename("~/Documents",EmailTemplateType.ReportAcknowledged);
        Assert.Equal("~/Documents/ReportAcknowledged.txt", templateFilename);
    }
}