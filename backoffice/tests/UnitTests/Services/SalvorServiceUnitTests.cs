using AutoMapper;
using Droits.Data;
using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Models.FormModels.SearchFormModels;
using Droits.Models.ViewModels;
using Droits.Models.ViewModels.ListViews;
using Droits.Repositories;
using Droits.Services;
using Microsoft.EntityFrameworkCore;

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
        public async Task GetSalvorListViewAsync_ReturnsDroitsCountForEachSalvor()
        {
            await using var dbContext = CreateSalvorContext();
            var service = CreateSalvorService(dbContext);

            var result = await service.GetSalvorListViewAsync(new SearchOptions
            {
                IncludeAssociations = true,
                PageSize = 10
            });

            var countsByName = result.Items.Cast<SalvorView>().ToDictionary(salvor => salvor.Name, salvor => salvor.DroitsCount);

            Assert.Equal(3, countsByName["Salvor with three droits"]);
            Assert.Equal(2, countsByName["Salvor with two droits"]);
            Assert.Equal(0, countsByName["Salvor without droits"]);
        }


        [Fact]
        public async Task AdvancedSearchAsync_SortsByDroitsCountDescendingAndPaginates()
        {
            await using var dbContext = CreateSalvorContext();
            var service = CreateSalvorService(dbContext);

            var firstPage = await service.AdvancedSearchAsync(new SalvorSearchForm
            {
                OrderColumn = nameof(SalvorView.DroitsCount),
                OrderDescending = true,
                PageSize = 2
            });
            var secondPage = await service.AdvancedSearchAsync(new SalvorSearchForm
            {
                OrderColumn = nameof(SalvorView.DroitsCount),
                OrderDescending = true,
                PageNumber = 2,
                PageSize = 2
            });

            Assert.Equal(3, firstPage.TotalCount);
            Assert.Equal(new[] { 3, 2 }, firstPage.Items.Cast<SalvorView>().Select(salvor => salvor.DroitsCount));
            Assert.Equal(2, secondPage.PageNumber);
            Assert.Equal(new[] { 0 }, secondPage.Items.Cast<SalvorView>().Select(salvor => salvor.DroitsCount));
        }


        [Fact]
        public async Task AdvancedSearchAsync_SortsByDroitsCountAscending()
        {
            await using var dbContext = CreateSalvorContext();
            var service = CreateSalvorService(dbContext);

            var result = await service.AdvancedSearchAsync(new SalvorSearchForm
            {
                OrderColumn = nameof(SalvorView.DroitsCount),
                OrderDescending = false,
                PageSize = 10
            });

            Assert.Equal(new[] { 0, 2, 3 }, result.Items.Cast<SalvorView>().Select(salvor => salvor.DroitsCount));
        }


        [Fact]
        public async Task AdvancedSearchAsync_SortsByNameAscending()
        {
            await using var dbContext = CreateSalvorContext();
            var service = CreateSalvorService(dbContext);

            var result = await service.AdvancedSearchAsync(new SalvorSearchForm
            {
                OrderColumn = nameof(SalvorView.Name),
                OrderDescending = false,
                PageSize = 10
            });

            Assert.Equal(
                new[] { "Salvor with three droits", "Salvor with two droits", "Salvor without droits" },
                result.Items.Cast<SalvorView>().Select(salvor => salvor.Name));
        }


        [Fact]
        public async Task AdvancedSearchAsync_SortsByNameDescending()
        {
            await using var dbContext = CreateSalvorContext();
            var service = CreateSalvorService(dbContext);

            var result = await service.AdvancedSearchAsync(new SalvorSearchForm
            {
                OrderColumn = nameof(SalvorView.Name),
                OrderDescending = true,
                PageSize = 10
            });

            Assert.Equal(
                new[] { "Salvor without droits", "Salvor with two droits", "Salvor with three droits" },
                result.Items.Cast<SalvorView>().Select(salvor => salvor.Name));
        }


        private static DroitsContext CreateSalvorContext()
        {
            var dbContext = new DroitsContext(new DbContextOptionsBuilder<DroitsContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);
            var salvors = new[]
            {
                new Salvor { Id = Guid.NewGuid(), Name = "Salvor with three droits", Email = "three@example.test" },
                new Salvor { Id = Guid.NewGuid(), Name = "Salvor with two droits", Email = "two@example.test" },
                new Salvor { Id = Guid.NewGuid(), Name = "Salvor without droits", Email = "none@example.test" }
            };

            dbContext.Salvors.AddRange(salvors);
            dbContext.Droits.AddRange(
                CreateDroits(salvors[0], 3)
                    .Concat(CreateDroits(salvors[1], 2)));
            dbContext.SaveChanges();

            return dbContext;
        }


        private static IEnumerable<Droit> CreateDroits(Salvor salvor, int count)
        {
            return Enumerable.Range(1, count).Select(index => new Droit
            {
                Id = Guid.NewGuid(),
                Reference = $"{salvor.Name}-{index}",
                SalvorId = salvor.Id
            });
        }


        private static SalvorService CreateSalvorService(DroitsContext dbContext)
        {
            return new SalvorService(
                new SalvorRepository(dbContext, Mock.Of<IAccountService>()),
                Mock.Of<IMapper>());
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
