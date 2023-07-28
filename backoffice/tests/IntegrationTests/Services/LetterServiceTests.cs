using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Droits.Clients;
using Droits.Models.Entities;
using Droits.Models.ViewModels;
using Droits.Repositories;
using Droits.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Droits.Tests.IntegrationTests.Services
{
    public class LetterServiceTests : IClassFixture<TestFixture>
    {
        private readonly ILetterService _service;
        private readonly Mock<ILetterRepository> _mockLetterRepository;

        private readonly Guid _letterId = Guid.NewGuid();
        private readonly DateTime _todaysDate = DateTime.Now;

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
        public async Task SendLetterAsync_ShouldReturnAValidResponse()
        {
            // Arrange
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

            // Act
            var response = await _service.SendLetterAsync(_letterId);

            // Assert
            Assert.NotNull(response);
        }

        [Fact]
        public async Task SendLetterAsync_ShouldReturnSubmittedTextInTheBody()
        {
            // Arrange
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

            // Act
            var response = await _service.SendLetterAsync(_letterId);

            // Assert
            Assert.Equal("This is a test", response.content.body);
        }

        [Fact]
        public async Task SendLetterAsync_ShouldReturnSubmittedSubjectInTheSubject()
        {
            // Arrange
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

            // Act
            var response = await _service.SendLetterAsync(_letterId);

            // Assert
            Assert.Equal("Wreckage Found!", response.content.subject);
        }

        [Fact]
        public async Task SendLetterAsync_ShouldInvokeTheUpdateMethodInTheRepository()
        {
            // Arrange
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

            // Act
            await _service.SendLetterAsync(_letterId);

            // Assert
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

            IQueryable<Letter> letters = lettersInRepo.AsQueryable();

            _mockLetterRepository.Setup(m => m.GetLetters()).Returns(letters);
        }
    }
}
