using AutoMapper;
using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Repositories;
using Droits.Services;

namespace Droits.Tests.UnitTests.Services
{
    public class SalvorServiceUnitTests
    {
        private readonly Mock<ISalvorRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly SalvorService _service;

        public SalvorServiceUnitTests()
        {
            _mockRepo = new Mock<ISalvorRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new SalvorService(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task SaveSalvorAsync_NewSalvor_AddsSalvor()
        {
            // Given
            var newSalvor = new Salvor { Name = "NewSalvor" };
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Salvor>(),It.IsAny<bool>())).ReturnsAsync(newSalvor);

            // When
            var result = await _service.SaveSalvorAsync(newSalvor);

            // Then
            Assert.Equal(newSalvor, result);
            _mockRepo.Verify(r => r.AddAsync(newSalvor, true), Times.Once);
        }


        [Fact]
        public async Task SaveSalvorAsync_NewSalvor_AddsSalvor_Duplicate()
        {
            // Given
            var newSalvor = new Salvor { Name = "NewSalvor", Email = "email@duplicate.com" };
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Salvor>(), It.IsAny<bool>()))
                .ReturnsAsync(newSalvor);
            _mockRepo.Setup(r => r.GetSalvorByEmailAddressAsync(It.IsAny<string>()))
                .ReturnsAsync(new Salvor(){Name = "Already existing salvor", Email = "email@duplicate.com"});

            // When & Assert
            await Assert.ThrowsAsync<DuplicateSalvorException>(() =>
                _service.SaveSalvorAsync(newSalvor));
        }


        [Fact]
        public async Task UpdateSalvorAsync_ExistingSalvor_UpdatesSalvor()
        {
            // Given
            var existingSalvor = new Salvor { Id = Guid.NewGuid(), Name = "ExistingSalvor" };
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Salvor>(),It.IsAny<bool>())).ReturnsAsync(existingSalvor);

            // When
            var result = await _service.SaveSalvorAsync(existingSalvor);

            // Then
            Assert.Equal(existingSalvor, result);
            _mockRepo.Verify(r => r.UpdateAsync(existingSalvor, true), Times.Once);
        }

        [Fact]
        public async Task GetSalvorAsync_ExistingId_ReturnsSalvor()
        {
            // Given
            var salvorId = Guid.NewGuid();
            var expectedSalvor = new Salvor { Id = salvorId, Name = "TestSalvor" };
            _mockRepo.Setup(r => r.GetSalvorAsync(salvorId)).ReturnsAsync(expectedSalvor);

            // When
            var result = await _service.GetSalvorAsync(salvorId);

            // Then
            Assert.Equal(expectedSalvor, result);
        }

        [Fact]
        public async Task SaveSalvorFormAsync_NewSalvorForm_AddsSalvor()
        {
            // Given
            var salvorForm = new SalvorForm { Name = "NewSalvorForm" };
            var newSalvor = new Salvor { Name = "NewSalvor" };
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Salvor>(),It.IsAny<bool>())).ReturnsAsync(newSalvor);

            // When
            var result = await _service.SaveSalvorFormAsync(salvorForm);

            // Then
            Assert.Equal(newSalvor.Id, result);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Salvor>(),It.IsAny<bool>()), Times.Once);
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Salvor>(),It.IsAny<bool>()), Times.Never);
        }

    }
}
