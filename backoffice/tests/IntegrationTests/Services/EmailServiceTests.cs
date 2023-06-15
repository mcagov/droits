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
        var form = new EmailForm()
        {
            EmailId = _emailId,
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
            EmailId = _emailId,
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
        SeedMockDatabase();
        
        EmailForm form = new()
        {
            EmailId = _emailId,
            EmailAddress = "sam.kendell+testing@madetech.com",
            Subject = "Wreckage Found!",
            Body = "This is a test"
        };

        Email testEmail = new()
        {
            Id = _emailId,
            Recipient = form.EmailAddress,
            Subject = form.Subject,
            Body = form.Body,
            DateCreated = _todaysDate,
            DateLastModified = _todaysDate
        };
        
        _mockEmailRepository.Setup(m => m.GetEmailAsync(_emailId)).Returns(Task.FromResult(testEmail));
        _mockEmailRepository.Setup(m => m.UpdateEmailAsync(testEmail));
        
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
            EmailAddress = "sam.kendell+testing@madetech.com",
            Subject = "Wreckage Found!",
            Body = "This is a test"
        };

        Email testEmail = new()
        {
            Id = _emailId,
            Recipient = form.EmailAddress,
            Subject = form.Subject,
            Body = form.Body,
            DateCreated = _todaysDate,
            DateLastModified = _todaysDate
        };
        
        _mockEmailRepository.Setup(m => m.GetEmailAsync(_emailId)).Returns(Task.FromResult(testEmail));
        _mockEmailRepository.Setup(m => m.UpdateEmailAsync(testEmail));
        
        await _service.SendEmailAsync(form);
        
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
            DateCreated = _todaysDate,
            DateLastModified = _todaysDate
        };
        
        Email paintingEmail = new()
        {
            Id = Guid.NewGuid(),
            Recipient = "denise@gmail.com",
            Subject = "Painting",
            Body = "I found a medieval painting washed up on the shore",
            DateCreated = _todaysDate,
            DateLastModified = _todaysDate
        };
        
        Email testEmail = new()
        {
            Id = _emailId,
            Recipient = "sam.kendell+testing@madetech.com",
            Subject = "Wreckage Found!",
            Body = "This is a test",
            DateCreated = _todaysDate,
            DateLastModified = _todaysDate
        };

        List<Email> emailsInRepo = new List<Email>()
        {
            shipwreckEmail,
            paintingEmail,
            testEmail
        };

        Task<Email> singleEmail = Task.FromResult(shipwreckEmail);

        _mockEmailRepository.Setup(m => m.GetEmails()).Returns(emailsInRepo);
    }
}