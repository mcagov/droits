using Droits.Clients;
using Droits.Models;
using Droits.Models.Domain;
using Droits.Services;
using Droits.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Notify.Models.Responses;

namespace Droits.Tests.IntegrationTests.Services;

public class EmailServiceTests
{
    private readonly IEmailService _service;
    private readonly Mock<IEmailRepository> _mockEmailRepository;
    public Guid _emailId = Guid.NewGuid();

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
        var form = new EmailForm()
        {
            EmailId = _emailId,
            Recipient = "sam.kendell+testing@madetech.com",
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
            EmailId = _emailId,
            Recipient = "sam.kendell+testing@madetech.com",
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
            EmailId = _emailId,
            Recipient = "sam.kendell+testing@madetech.com",
            Subject = "Test",
            Body = "This is a test"
        };
        
        var response = await _service.SendEmailAsync(form);

        Assert.Equal("Test",response.content.subject);
    }
    
    [Fact]
    public async void SendEmailAsync_ShouldInvokeTheUpdateMethodInTheRepository()
    {
        SeedMockDatabase();
        
        
        EmailForm form = new()
        {
            EmailId = _emailId,
            Recipient = "sam.kendell+testing@madetech.com",
            Subject = "Wreckage Found!",
            Body = "This is a test"
        };

        Email sentEmail = new()
        {
            Id = _emailId,
            Recipient = form.Recipient,
            Subject = form.Subject,
            Body = form.Body
        };

        _mockEmailRepository.Setup(m => m.UpdateEmailAsync(sentEmail));
        EmailNotificationResponse response = await _service.SendEmailAsync(form);
        
        _mockEmailRepository.Verify(m => m.UpdateEmailAsync(sentEmail), Times.Once);
    }

    private void SeedMockDatabase()
    {
        DateTime todaysDate = DateTime.Now;
        
        Email shipwreckEmail = new()
        {
            Id = _emailId,
            Recipient = "barry@gmail.com",
            Subject = "Shipwreck",
            Body = "I found an old ship on the beach",
            DateCreated = todaysDate,
            DateLastModified = todaysDate
        };
        
        Email paintingEmail = new()
        {
            Id = Guid.NewGuid(),
            Recipient = "denise@gmail.com",
            Subject = "Painting",
            Body = "I found a medieval painting washed up on the shore",
            DateCreated = todaysDate,
            DateLastModified = todaysDate
        };

        List<Email> emailsInRepo = new List<Email>()
        {
            shipwreckEmail,
            paintingEmail
        };

        Task<Email> singleEmail = Task.FromResult(shipwreckEmail);

        _mockEmailRepository.Setup(m => m.GetEmails()).Returns(emailsInRepo);
        _mockEmailRepository.Setup(m => m.GetEmailAsync(_emailId)).Returns(singleEmail);
    }
}