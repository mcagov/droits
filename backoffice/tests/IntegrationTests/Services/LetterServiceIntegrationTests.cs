using Droits.Clients;
using Droits.Models.Entities;
using Droits.Repositories;
using Droits.Services;
using Droits.Tests.Helpers;
using Microsoft.Extensions.Logging;


namespace Droits.Tests.IntegrationTests.Services
{
    public class LetterServiceIntegrationTests : IClassFixture<TestFixture>
    {
        private readonly ILetterService _service;
        private readonly Mock<ILetterRepository> _mockLetterRepository;

        private readonly Guid _letterId = Guid.NewGuid();
        private readonly DateTime _todaysDate = DateTime.Now;

        public LetterServiceIntegrationTests(TestFixture fixture)
        {
            var logger = new Mock<ILogger<LetterService>>();
            _mockLetterRepository = new Mock<ILetterRepository>();
            Mock<IDroitService> mockDroitService = new();

            var configuration = fixture.Configuration;
            var client = new GovNotifyClient(new Mock<ILogger<GovNotifyClient>>().Object, configuration);

            _service = new LetterService(logger.Object, client, _mockLetterRepository.Object, mockDroitService.Object);
        }

        [Fact]
        public async Task SendLetterAsync_ShouldReturnAValidResponse()
        {
            // Given
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

            _mockLetterRepository.Setup(m => m.GetLetterAsync(_letterId))
                .Returns(Task.FromResult(testLetter));
            _mockLetterRepository.Setup(m => m.UpdateAsync(testLetter));

            // When
            var response = await _service.SendLetterAsync(_letterId);

            // Then
            Assert.NotNull(response);
        }

        [Fact]
        public async Task SendLetterAsync_ShouldReturnSubmittedTextInTheBody()
        {
            // Given
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

            _mockLetterRepository.Setup(m => m.GetLetterAsync(_letterId))
                .Returns(Task.FromResult(testLetter));
            _mockLetterRepository.Setup(m => m.UpdateAsync(testLetter));

            // When
            var response = await _service.SendLetterAsync(_letterId);

            // Then
            Assert.Equal("This is a test", response.content.body);
        }

        [Fact]
        public async Task SendLetterAsync_ShouldReturnSubmittedSubjectInTheSubject()
        {
            // Given
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

            _mockLetterRepository.Setup(m => m.GetLetterAsync(_letterId))
                .Returns(Task.FromResult(testLetter));
            _mockLetterRepository.Setup(m => m.UpdateAsync(testLetter));

            // When
            var response = await _service.SendLetterAsync(_letterId);

            // Then
            Assert.Equal("Wreckage Found!", response.content.subject);
        }

        [Fact]
        public async Task SendLetterAsync_ShouldInvokeTheUpdateMethodInTheRepository()
        {
            // Given
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

            _mockLetterRepository.Setup(m => m.GetLetterAsync(_letterId))
                .Returns(Task.FromResult(testLetter));
            _mockLetterRepository.Setup(m => m.UpdateAsync(testLetter));

            // When
            await _service.SendLetterAsync(_letterId);

            // Then
            _mockLetterRepository.Verify(m => m.UpdateAsync(testLetter), Times.Once);
        }

        private void SeedMockDatabase()
        {
            // Mock database seeding code
        }
    }
}
