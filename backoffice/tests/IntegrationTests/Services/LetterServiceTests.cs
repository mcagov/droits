using Droits.Clients;
using Droits.Models.Entities;
using Droits.Services;
using Droits.Repositories;
using Microsoft.Extensions.Logging;

namespace Droits.Tests.IntegrationTests.Services;

public class LetterServiceTests : IClassFixture<TestFixture>
{
    private readonly ILetterService _service;
    private readonly Mock<ILetterRepository> _mockLetterRepository;

    private Guid _letterId = Guid.NewGuid();
    private DateTime _todaysDate = DateTime.Now;


    public LetterServiceTests(TestFixture fixture)
    {
        var logger = new Mock<ILogger<LetterService>>();
        _mockLetterRepository = new Mock<ILetterRepository>();
        Mock<IDroitService> mockDroitService = new Mock<IDroitService>();



        var configuration = fixture.Configuration;

        var client = new GovNotifyClient(new Mock<ILogger<GovNotifyClient>>().Object, configuration);

        _service = new LetterService(logger.Object, client, _mockLetterRepository.Object, mockDroitService.Object);
    }

    [Fact]
    public async void SendLetterAsync_ShouldReturnAValidResponse()
    {
        SeedMockDatabase();

        Letter testLetter = new()
        {
            Id = _letterId,
            Recipient = "sam.kendell+testing@madetech.com",
            Subject = "Wreckage Found!",
            Body = "This is a test",
            Created = _todaysDate,
            LastModified = _todaysDate
        };

        _mockLetterRepository.Setup(m => m.GetLetterAsync(_letterId)).Returns(Task.FromResult(testLetter));
        _mockLetterRepository.Setup(m => m.UpdateLetterAsync(testLetter));


        var response = await _service.SendLetterAsync(_letterId);

        Assert.NotNull(response);
    }
    [Fact]
    public async void SendLetterAsync_ShouldReturnSubmittedTextInTheBody()
    {
        SeedMockDatabase();
        Letter testLetter = new()
        {
            Id = _letterId,
            Recipient = "sam.kendell+testing@madetech.com",
            Subject = "Wreckage Found!",
            Body = "This is a test",
            Created = _todaysDate,
            LastModified = _todaysDate
        };

        _mockLetterRepository.Setup(m => m.GetLetterAsync(_letterId)).Returns(Task.FromResult(testLetter));
        _mockLetterRepository.Setup(m => m.UpdateLetterAsync(testLetter));

        var response = await _service.SendLetterAsync(_letterId);

        Assert.Equal("This is a test",response.content.body);
    }
    [Fact]
    public async void SendLetterAsync_ShouldReturnSubmittedSubjectInTheSubject()
    {
        SeedMockDatabase();

        Letter testLetter = new()
        {
            Id = _letterId,
            Recipient = "sam.kendell+testing@madetech.com",
            Subject = "Wreckage Found!",
            Body = "This is a test",
            Created = _todaysDate,
            LastModified = _todaysDate
        };

        _mockLetterRepository.Setup(m => m.GetLetterAsync(_letterId)).Returns(Task.FromResult(testLetter));
        _mockLetterRepository.Setup(m => m.UpdateLetterAsync(testLetter));

        var response = await _service.SendLetterAsync(_letterId);

        Assert.Equal("Wreckage Found!",response.content.subject);
    }

    [Fact]
    public async void SendLetterAsync_ShouldInvokeTheUpdateMethodInTheRepository()
    {
        SeedMockDatabase();

        Letter testLetter = new()
        {
            Id = _letterId,
            Recipient = "sam.kendell+testing@madetech.com",
            Subject = "Wreckage Found!",
            Body = "This is a test",
            Created = _todaysDate,
            LastModified = _todaysDate
        };

        _mockLetterRepository.Setup(m => m.GetLetterAsync(_letterId)).Returns(Task.FromResult(testLetter));
        _mockLetterRepository.Setup(m => m.UpdateLetterAsync(testLetter));

        await _service.SendLetterAsync(_letterId);

        _mockLetterRepository.Verify(m => m.UpdateLetterAsync(testLetter), Times.Once);
    }

    private void SeedMockDatabase()
    {
        Letter shipwreckLetter = new()
        {
            Id = Guid.NewGuid(),
            Recipient = "barry@gmail.com",
            Subject = "Shipwreck",
            Body = "I found an old ship on the beach",
            Created = _todaysDate,
            LastModified = _todaysDate
        };

        Letter paintingLetter = new()
        {
            Id = Guid.NewGuid(),
            Recipient = "denise@gmail.com",
            Subject = "Painting",
            Body = "I found a medieval painting washed up on the shore",
            Created = _todaysDate,
            LastModified = _todaysDate
        };

        Letter testLetter = new()
        {
            Id = _letterId,
            Recipient = "sam.kendell+testing@madetech.com",
            Subject = "Wreckage Found!",
            Body = "This is a test",
            Created = _todaysDate,
            LastModified = _todaysDate
        };

        List<Letter> lettersInRepo = new List<Letter>()
        {
            shipwreckLetter,
            paintingLetter,
            testLetter
        };

        Task<List<Letter>> letters = Task.FromResult(lettersInRepo);

        _mockLetterRepository.Setup(m => m.GetLettersAsync()).Returns(letters);
    }
}
