using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Repositories;
using Droits.Services;
using Droits.Data;

namespace Droits.Tests.UnitTests.Services
{
    public class WreckServiceUnitTests
    {
        private readonly Mock<IWreckRepository> _mockRepo;
        private readonly WreckService _service;

        public WreckServiceUnitTests()
        {
            _mockRepo = new Mock<IWreckRepository>();
            _service = new WreckService(_mockRepo.Object);
        }
        
        [Fact]
        public async Task SaveWreckAsync_NewWreck_AddsWreck()
        {
            // Given
            var newWreck = new Wreck { Name = "NewWreck" };
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Wreck>(),It.IsAny<bool>())).ReturnsAsync(newWreck);

            // When
            var result = await _service.SaveWreckAsync(newWreck);

            // Then
            Assert.Equal(newWreck, result);
            _mockRepo.Verify(r => r.AddAsync(newWreck, true), Times.Once);
        }

        [Fact]
        public async Task UpdateWreckAsync_ExistingWreck_UpdatesWreck()
        {
            // Given
            var existingWreck = new Wreck { Id = Guid.NewGuid(), Name = "ExistingWreck" };
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Wreck>(),It.IsAny<bool>())).ReturnsAsync(existingWreck);

            // When
            var result = await _service.SaveWreckAsync(existingWreck);

            // Then
            Assert.Equal(existingWreck, result);
            _mockRepo.Verify(r => r.UpdateAsync(existingWreck, true), Times.Once);
        }

        [Fact]
        public async Task GetWreckAsync_ExistingId_ReturnsWreck()
        {
            // Given
            var wreckId = Guid.NewGuid();
            var expectedWreck = new Wreck { Id = wreckId, Name = "TestWreck" };
            _mockRepo.Setup(r => r.GetWreckAsync(wreckId)).ReturnsAsync(expectedWreck);

            // When
            var result = await _service.GetWreckAsync(wreckId);

            // Then
            Assert.Equal(expectedWreck, result);
        }

        [Fact]
        public async Task SaveWreckFormAsync_NewWreckForm_AddsWreck()
        {
            // Given
            var wreckForm = new WreckForm { Name = "NewWreckForm" };
            var newWreck = new Wreck { Name = "NewWreck" };
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Wreck>(),It.IsAny<bool>())).ReturnsAsync(newWreck);

            // When
            var result = await _service.SaveWreckFormAsync(wreckForm);

            // Then
            Assert.Equal(newWreck.Id, result);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Wreck>(),It.IsAny<bool>()), Times.Once);
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Wreck>(),It.IsAny<bool>()), Times.Never);

        }

    }
}
