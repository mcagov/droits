using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Models.FormModels.SearchFormModels;
using Droits.Models.ViewModels;
using Droits.Models.ViewModels.ListViews;
using Droits.Repositories;
using Droits.Services;
using Droits.Data;
using Microsoft.EntityFrameworkCore;

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
        public void WreckSearchForm_DefaultsToCreatedDescendingOrder()
        {
            var form = new WreckSearchForm();

            Assert.Equal("Created", form.OrderColumn);
            Assert.True(form.OrderDescending);
        }


        [Fact]
        public async Task GetWrecksListViewAsync_ReturnsDroitsCountForEachWreck()
        {
            await using var dbContext = CreateWreckContext();
            var service = CreateWreckService(dbContext);

            var result = await service.GetWrecksListViewAsync(new SearchOptions
            {
                IncludeAssociations = true,
                PageSize = 10
            });

            var countsByName = result.Items.Cast<WreckView>().ToDictionary(wreck => wreck.Name, wreck => wreck.DroitsCount);

            Assert.Equal(3, countsByName["Wreck with three droits"]);
            Assert.Equal(2, countsByName["Wreck with two droits"]);
            Assert.Equal(0, countsByName["Wreck without droits"]);
        }


        [Fact]
        public async Task AdvancedSearchAsync_SortsByDroitsCountDescendingAndPaginates()
        {
            await using var dbContext = CreateWreckContext();
            var service = CreateWreckService(dbContext);

            var firstPage = await service.AdvancedSearchAsync(new WreckSearchForm
            {
                OrderColumn = nameof(WreckView.DroitsCount),
                OrderDescending = true,
                PageSize = 2
            });
            var secondPage = await service.AdvancedSearchAsync(new WreckSearchForm
            {
                OrderColumn = nameof(WreckView.DroitsCount),
                OrderDescending = true,
                PageNumber = 2,
                PageSize = 2
            });

            Assert.Equal(3, firstPage.TotalCount);
            Assert.Equal(new[] { 3, 2 }, firstPage.Items.Cast<WreckView>().Select(wreck => wreck.DroitsCount));
            Assert.Equal(2, secondPage.PageNumber);
            Assert.Equal(new[] { 0 }, secondPage.Items.Cast<WreckView>().Select(wreck => wreck.DroitsCount));
        }


        [Fact]
        public async Task AdvancedSearchAsync_SortsByDroitsCountAscending()
        {
            await using var dbContext = CreateWreckContext();
            var service = CreateWreckService(dbContext);

            var result = await service.AdvancedSearchAsync(new WreckSearchForm
            {
                OrderColumn = nameof(WreckView.DroitsCount),
                OrderDescending = false,
                PageSize = 10
            });

            Assert.Equal(new[] { 0, 2, 3 }, result.Items.Cast<WreckView>().Select(wreck => wreck.DroitsCount));
        }


        [Fact]
        public async Task AdvancedSearchAsync_SortsByCreatedDescending()
        {
            await using var dbContext = CreateWreckContext();
            var service = CreateWreckService(dbContext);

            var result = await service.AdvancedSearchAsync(new WreckSearchForm
            {
                OrderColumn = nameof(WreckView.Created),
                OrderDescending = true,
                PageSize = 10
            });

            Assert.Equal(
                new[] { "Wreck with two droits", "Wreck without droits", "Wreck with three droits" },
                result.Items.Cast<WreckView>().Select(wreck => wreck.Name));
        }


        [Fact]
        public async Task AdvancedSearchAsync_SortsByCreatedAscending()
        {
            await using var dbContext = CreateWreckContext();
            var service = CreateWreckService(dbContext);

            var result = await service.AdvancedSearchAsync(new WreckSearchForm
            {
                OrderColumn = nameof(WreckView.Created),
                OrderDescending = false,
                PageSize = 10
            });

            Assert.Equal(
                new[] { "Wreck with three droits", "Wreck without droits", "Wreck with two droits" },
                result.Items.Cast<WreckView>().Select(wreck => wreck.Name));
        }


        private static DroitsContext CreateWreckContext()
        {
            var dbContext = new DroitsContext(new DbContextOptionsBuilder<DroitsContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);
            var wrecks = new[]
            {
                new Wreck { Id = Guid.NewGuid(), Name = "Wreck with three droits", Created = new DateTime(2026, 3, 1) },
                new Wreck { Id = Guid.NewGuid(), Name = "Wreck with two droits", Created = new DateTime(2026, 3, 3) },
                new Wreck { Id = Guid.NewGuid(), Name = "Wreck without droits", Created = new DateTime(2026, 3, 2) }
            };

            dbContext.Wrecks.AddRange(wrecks);
            dbContext.Droits.AddRange(
                CreateDroits(wrecks[0], 3)
                    .Concat(CreateDroits(wrecks[1], 2)));
            dbContext.SaveChanges();

            return dbContext;
        }


        private static IEnumerable<Droit> CreateDroits(Wreck wreck, int count)
        {
            return Enumerable.Range(1, count).Select(index => new Droit
            {
                Id = Guid.NewGuid(),
                Reference = $"{wreck.Name}-{index}",
                WreckId = wreck.Id
            });
        }


        private static WreckService CreateWreckService(DroitsContext dbContext)
        {
            return new WreckService(new WreckRepository(dbContext, Mock.Of<IAccountService>()));
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
