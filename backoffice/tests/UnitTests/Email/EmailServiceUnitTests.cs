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
    
    
    
}