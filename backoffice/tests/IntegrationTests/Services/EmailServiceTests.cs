using Droits.Clients;
using Droits.Models.Entities;
using Droits.Services;
using Droits.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Droits.Tests.IntegrationTests.Services;

public class EmailServiceTests
{
    private readonly IEmailService _service;
    private readonly Mock<IEmailRepository> _mockEmailRepository;

    private Guid _emailId = Guid.NewGuid();
    private DateTime _todaysDate = DateTime.Now;


    public EmailServiceTests()
    {
        var logger = new Mock<ILogger<EmailService>>();
        _mockEmailRepository = new Mock<IEmailRepository>();

        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@"appsettings.Development.json", false, false)
            .AddEnvironmentVariables()
            .Build();

        var client = new GovNotifyClient(new Mock<ILogger<GovNotifyClient>>().Object, config);

        _service = new EmailService(logger.Object, client, _mockEmailRepository.Object);
    }

    [Fact]
    public async void SendEmailAsync_ShouldReturnAValidResponse()
    {
        SeedMockDatabase();

        Email testEmail = new()
        {
            Id = _emailId,
            Recipient = "sam.kendell+testing@madetech.com",
            Subject = "Wreckage Found!",
            Body = "This is a test",
            Created = _todaysDate,
            LastModified = _todaysDate
        };

        _mockEmailRepository.Setup(m => m.GetEmailAsync(_emailId)).Returns(Task.FromResult(testEmail));
        _mockEmailRepository.Setup(m => m.UpdateEmailAsync(testEmail));


        var response = await _service.SendEmailAsync(_emailId);

        Assert.NotNull(response);
    }
    [Fact]
    public async void SendEmailAsync_ShouldReturnSubmittedTextInTheBody()
    {
        SeedMockDatabase();
        Email testEmail = new()
        {
            Id = _emailId,
            Recipient = "sam.kendell+testing@madetech.com",
            Subject = "Wreckage Found!",
            Body = "This is a test",
            Created = _todaysDate,
            LastModified = _todaysDate
        };

        _mockEmailRepository.Setup(m => m.GetEmailAsync(_emailId)).Returns(Task.FromResult(testEmail));
        _mockEmailRepository.Setup(m => m.UpdateEmailAsync(testEmail));

        var response = await _service.SendEmailAsync(_emailId);

        Assert.Equal("This is a test",response.content.body);
    }
    [Fact]
    public async void SendEmailAsync_ShouldReturnSubmittedSubjectInTheSubject()
    {
        SeedMockDatabase();

        Email testEmail = new()
        {
            Id = _emailId,
            Recipient = "sam.kendell+testing@madetech.com",
            Subject = "Wreckage Found!",
            Body = "This is a test",
            Created = _todaysDate,
            LastModified = _todaysDate
        };

        _mockEmailRepository.Setup(m => m.GetEmailAsync(_emailId)).Returns(Task.FromResult(testEmail));
        _mockEmailRepository.Setup(m => m.UpdateEmailAsync(testEmail));

        var response = await _service.SendEmailAsync(_emailId);

        Assert.Equal("Wreckage Found!",response.content.subject);
    }

    [Fact]
    public async void SendEmailAsync_ShouldInvokeTheUpdateMethodInTheRepository()
    {
        SeedMockDatabase();

        Email testEmail = new()
        {
            Id = _emailId,
            Recipient = "sam.kendell+testing@madetech.com",
            Subject = "Wreckage Found!",
            Body = "This is a test",
            Created = _todaysDate,
            LastModified = _todaysDate
        };

        _mockEmailRepository.Setup(m => m.GetEmailAsync(_emailId)).Returns(Task.FromResult(testEmail));
        _mockEmailRepository.Setup(m => m.UpdateEmailAsync(testEmail));

        await _service.SendEmailAsync(_emailId);

        _mockEmailRepository.Verify(m => m.UpdateEmailAsync(testEmail), Times.Once);
    }

    private void SeedMockDatabase()
    {
        Email shipwreckEmail = new()
        {
            Id = Guid.NewGuid(),
            Recipient = "barry@gmail.com",
            Subject = "Shipwreck",
            Body = "I found an old ship on the beach",
            Created = _todaysDate,
            LastModified = _todaysDate
        };

        Email paintingEmail = new()
        {
            Id = Guid.NewGuid(),
            Recipient = "denise@gmail.com",
            Subject = "Painting",
            Body = "I found a medieval painting washed up on the shore",
            Created = _todaysDate,
            LastModified = _todaysDate
        };

        Email testEmail = new()
        {
            Id = _emailId,
            Recipient = "sam.kendell+testing@madetech.com",
            Subject = "Wreckage Found!",
            Body = "This is a test",
            Created = _todaysDate,
            LastModified = _todaysDate
        };

        List<Email> emailsInRepo = new List<Email>()
        {
            shipwreckEmail,
            paintingEmail,
            testEmail
        };

        Task<List<Email>> emails = Task.FromResult(emailsInRepo);

        _mockEmailRepository.Setup(m => m.GetEmailsAsync()).Returns(emails);
    }
}
