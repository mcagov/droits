using AutoMapper;
using Droits.Data;
using Droits.Exceptions;
using Droits.Helpers;
using Droits.Models.DTOs;
using Droits.Models.DTOs.Exports;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Models.FormModels;
using Droits.Models.ViewModels;
using Droits.Models.FormModels.SearchFormModels;
using Droits.Models.ViewModels.ListViews;
using Droits.Repositories;
using Droits.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Droits.Tests.UnitTests.Services
{
    public class DroitServiceUnitTests
    {
        private readonly Mock<IDroitRepository> _mockRepo;
        private readonly DroitService _service;

        public DroitServiceUnitTests()
        {
            _mockRepo = new Mock<IDroitRepository>();
            Mock<IWreckMaterialService> mockWreckMaterialService = new();
            Mock<IAccountService> mockCurrentUserService = new();
            Mock<ILogger<DroitService>> mockLogger = new();
            var mockMapper = new Mock<IMapper>();
            _service = new DroitService(mockLogger.Object, _mockRepo.Object, mockWreckMaterialService.Object, mockCurrentUserService.Object, mockMapper.Object);
        }

        [Fact]
        public async Task GetWreckDroitsListViewAsync_ReturnsOnlyWreckDroitsAndTotalCount()
        {
            var testData = CreateWreckDroitsContext();
            await using var dbContext = testData.DbContext;
            var service = CreateDroitService(dbContext);

            var result = await service.GetWreckDroitsListViewAsync(testData.WreckId, new SearchOptions
            {
                PageSize = 10
            });

            var references = result.Items.Cast<DroitView>().Select(d => d.Reference).ToList();

            Assert.Equal(3, result.TotalCount);
            Assert.Equal(3, references.Count);
            Assert.DoesNotContain("OTHER-001", references);
        }


        [Fact]
        public async Task GetWreckDroitsListViewAsync_ReturnsRequestedPagesInReportedDateOrder()
        {
            var testData = CreateWreckDroitsContext();
            await using var dbContext = testData.DbContext;
            var service = CreateDroitService(dbContext);

            var firstPage = await service.GetWreckDroitsListViewAsync(testData.WreckId, new SearchOptions
            {
                PageNumber = 1,
                PageSize = 2
            });
            var secondPage = await service.GetWreckDroitsListViewAsync(testData.WreckId, new SearchOptions
            {
                PageNumber = 2,
                PageSize = 2
            });

            Assert.Equal(1, firstPage.PageNumber);
            Assert.Equal(new[] { "D-003", "D-002" }, firstPage.Items.Cast<DroitView>().Select(d => d.Reference));
            Assert.Equal(2, secondPage.PageNumber);
            Assert.Equal(new[] { "D-001" }, secondPage.Items.Cast<DroitView>().Select(d => d.Reference));
        }


        private static (DroitsContext DbContext, Guid WreckId) CreateWreckDroitsContext()
        {
            var wreckId = Guid.NewGuid();
            var dbContext = new DroitsContext(new DbContextOptionsBuilder<DroitsContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);

            dbContext.Droits.AddRange(
                new Droit
                {
                    Id = Guid.NewGuid(),
                    Reference = "D-003",
                    WreckId = wreckId,
                    ReportedDate = new DateTime(2026, 3, 3)
                },
                new Droit
                {
                    Id = Guid.NewGuid(),
                    Reference = "D-002",
                    WreckId = wreckId,
                    ReportedDate = new DateTime(2026, 3, 2)
                },
                new Droit
                {
                    Id = Guid.NewGuid(),
                    Reference = "D-001",
                    WreckId = wreckId,
                    ReportedDate = new DateTime(2026, 3, 1)
                },
                new Droit
                {
                    Id = Guid.NewGuid(),
                    Reference = "OTHER-001",
                    WreckId = Guid.NewGuid(),
                    ReportedDate = new DateTime(2026, 3, 4)
                });
            dbContext.SaveChanges();

            return (dbContext, wreckId);
        }


        private static DroitService CreateDroitService(DroitsContext dbContext)
        {
            var accountService = new Mock<IAccountService>();
            return new DroitService(
                Mock.Of<ILogger<DroitService>>(),
                new DroitRepository(dbContext, accountService.Object),
                Mock.Of<IWreckMaterialService>(),
                accountService.Object,
                Mock.Of<IMapper>());
        }


        [Fact]
        public async Task GetDroitAsync_ExistingId_ReturnsDroit()
        {
            // Given
            var droitId = Guid.NewGuid();
            var expectedDroit = new Droit { Id = droitId, Reference = "Ref2" };
            _mockRepo.Setup(r => r.GetDroitAsync(droitId)).ReturnsAsync(expectedDroit);

            // When
            var result = await _service.GetDroitAsync(droitId);

            // Then
            Assert.Equal(expectedDroit, result);
        }

        [Fact]
        public async Task GetDroitAsync_NonExistingId_ThrowsDroitNotFoundException()
        {
            // Given
            var droitId = Guid.NewGuid();
            _mockRepo.Setup(r => r.GetDroitAsync(droitId)).ThrowsAsync(new DroitNotFoundException());

            // When & Assert
            await Assert.ThrowsAsync<DroitNotFoundException>(() => _service.GetDroitAsync(droitId));
        }

        [Fact]
        public async Task SaveDroitAsync_NewDroit_AddsDroit()
        {
            // Given
            var newDroit = new Droit { Reference = "Ref3", Id = default(Guid)};
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Droit>(), true)).ReturnsAsync(newDroit);
            _mockRepo.Setup(r => r.GetDroitByReferenceAsync(It.IsAny<string>())).Throws(new DroitNotFoundException());

            // When
            var result = await _service.SaveDroitAsync(newDroit);

            // Then
            Assert.NotNull(result);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Droit>(),It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDroitStatusAsync_ValidIdAndStatus_UpdatesStatus()
        {
            // Given
            var droitId = Guid.NewGuid();
            var existingDroit = new Droit { Id = droitId, Status = DroitStatus.Received };
            _mockRepo.Setup(r => r.GetDroitAsync(droitId)).ReturnsAsync(existingDroit);

            // When
            await _service.UpdateDroitStatusAsync(droitId, DroitStatus.Research);
 
            // Then
            
            _mockRepo.Verify(r => r.UpdateAsync(It.Is<Droit>(d => d.Id == droitId && d.Status == DroitStatus.Research),It.IsAny<bool>()), Times.Once);
            Assert.Equal(DroitStatus.Research, existingDroit.Status);
        }


        [Fact]
        public async Task SaveDroitAsync_AddsDroit_WhenDroitIdIsDefault()
        {
            // Given
            var newDroit = new Droit { Reference = "TestRef", Id = default(Guid) };
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Droit>(), true)).ReturnsAsync(newDroit);
            _mockRepo.Setup(r => r.GetDroitByReferenceAsync(It.IsAny<string>())).Throws(new DroitNotFoundException());

            
            // When
            var result = await _service.SaveDroitAsync(newDroit);

            // Then
            _mockRepo.Verify(r => r.AddAsync(newDroit, true), Times.Once);
            Assert.Equal(newDroit, result);
        }

        [Fact]
        public async Task SaveDroitAsync_ExistingDroit_UpdatesDroit()
        {
            // Given
            var existingDroitId = Guid.NewGuid();
            var existingDroit = new Droit { Id = existingDroitId, Reference = "ExistingRef" };
            _mockRepo.Setup(r => r.GetDroitAsync(existingDroitId)).ReturnsAsync(existingDroit);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Droit>(), true)).ReturnsAsync(existingDroit);
            _mockRepo.Setup(r => r.IsReferenceUnique(It.IsAny<Droit>())).ReturnsAsync(true);

            // When
            var result = await _service.SaveDroitAsync(existingDroit);

            // Then
            _mockRepo.Verify(r => r.UpdateAsync(existingDroit, true), Times.Once);
            Assert.Equal(existingDroit, result);
        }

        [Fact]
        public async Task UpdateDroitStatusAsync_NonExistingId_ThrowsDroitNotFoundException()
        {
            // Given
            var nonExistingDroitId = Guid.NewGuid();
            _mockRepo.Setup(r => r.GetDroitAsync(nonExistingDroitId)).ThrowsAsync(new DroitNotFoundException());
            _mockRepo.Setup(r => r.IsReferenceUnique(It.IsAny<Droit>())).ReturnsAsync(true);


            // When & Assert
            await Assert.ThrowsAsync<DroitNotFoundException>(() => _service.UpdateDroitStatusAsync(nonExistingDroitId, DroitStatus.Research));
        }
        
        [Fact]
        public async Task ExportDroitsAsync_EmptyList_ThrowsException()
        {
            // Given
            var emptySearchform = new DroitSearchForm();
            _mockRepo.Setup(r => r.GetDroitsWithAssociations())
                .Returns(new List<Droit>().AsQueryable);

            // When & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.ExportAsync(emptySearchform));
        }
        
        [Fact]
        public async Task ExportDroitsAsync_ListOfDroits_ReturnsData()
        {
            // Given
            var droitSearchForm = new DroitSearchForm();
            var droitsQueryable = new List<Droit>()
            {
                new() {Id = Guid.NewGuid(), Reference = "Ref1"},
                new() {Id = Guid.NewGuid(), Reference = "Ref2"},
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetDroitsWithAssociations()).Returns(droitsQueryable);
            
            // When
            var data = await _service.ExportAsync(droitSearchForm);
            
            // Assert
            Assert.NotEmpty(data);
        }
        
        [Fact]
        public async Task ExportDroitsAsync_ListOfDroits_ReturnsCorrectData()
        {
            // Given
            var droitSearchForm = new DroitSearchForm();
            var droitsQueryable = new List<Droit>()
            {
                new() {Id = Guid.NewGuid(), Reference = "Ref1"},
                new() {Id = Guid.NewGuid(), Reference = "Ref2"},
            }.AsQueryable();

            _mockRepo.Setup(r => r.GetDroitsWithAssociations()).Returns(droitsQueryable);
            
            // When
            var data = await _service.ExportAsync(droitSearchForm);
            
            // Assert
            Assert.Contains("Ref1", System.Text.Encoding.UTF8.GetString(data));
            Assert.Contains("Ref2", System.Text.Encoding.UTF8.GetString(data));
            Assert.DoesNotContain("magna carta", System.Text.Encoding.UTF8.GetString(data));
        }
        
    }
    
}
