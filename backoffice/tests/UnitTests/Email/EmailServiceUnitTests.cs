using Droits.Clients;
using Droits.Repositories;
using Droits.Services;
using Microsoft.Extensions.Logging;

namespace Droits.Tests.UnitTests.Email;

public class EmailServiceUnitTests
{
    private readonly IEmailService _service;

    public EmailServiceUnitTests()
    {
        var mockLogger = new Mock<ILogger<EmailService>>();
        var mockClient = new Mock<IGovNotifyClient>();
        var mockEmailRepository = new Mock<IEmailRepository>();

        _service = new EmailService(
            mockLogger.Object,
            mockClient.Object,
            mockEmailRepository.Object);
    }
}