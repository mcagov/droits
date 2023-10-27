using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Repositories;
using Droits.Services;
using Droits.Tests.Helpers;
using Microsoft.Extensions.Logging;
using Droits.Data;
using Droits.Models.FormModels;
using Droits.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Droits.Tests.IntegrationTests.Services
{
    public class DroitServiceIntegrationTests : IClassFixture<TestFixture>
    {
        private readonly Mock<IAccountService> _mockCurrentUserService = new();
        private readonly Mock<ILogger<DroitService>> _mockLogger = new();
        private readonly Mock<IWreckMaterialService> _mockWmService = new();


        private readonly DroitService _service;
        private readonly DroitsContext _dbContext;

        public DroitServiceIntegrationTests(TestFixture fixture)
        {
            _dbContext = TestDbContextFactory.CreateDbContext();
            DatabaseSeeder.SeedData(_dbContext); // Seed the test data

            var droitRepository = new DroitRepository(_dbContext, _mockCurrentUserService.Object);
            _service = new DroitService(_mockLogger.Object, droitRepository, _mockWmService.Object, _mockCurrentUserService.Object);

        }
        [Fact]
        public async Task AddDroitAsync_ShouldAddNewDroitToDatabase()
        {
            // Arrange
            var newDroit = new Droit
            {
                Reference = "NewDroit",
                Status = DroitStatus.Received
            };

            // Act
            var addedDroit = await _service.SaveDroitAsync(newDroit);

            // Assert
            var retrievedDroit = await _dbContext.Droits.FindAsync(addedDroit.Id);
            Assert.NotNull(retrievedDroit);
            Assert.Equal(newDroit.Reference, retrievedDroit.Reference);
            Assert.Equal(newDroit.Status, retrievedDroit.Status);
        }

        [Fact]
        public async Task UpdateDroitAsync_ShouldUpdateExistingDroitInDatabase()
        {
            // Arrange
            var existingDroit = await _dbContext.Droits.FirstOrDefaultAsync();
            var updatedStatus = DroitStatus.Received;

            // Act
            existingDroit!.Status = updatedStatus;
            await _service.SaveDroitAsync(existingDroit);

            // Assert
            var retrievedDroit = await _dbContext.Droits.FindAsync(existingDroit.Id);
            Assert.NotNull(retrievedDroit);
            Assert.Equal(updatedStatus, retrievedDroit.Status);
        }
        
        //Search tests
        //
        // [Fact]
        // public async Task AdvancedSearchDroitsAsync_ShouldReturnCorrectMatches()
        // {
        //     // Arrange
        //     await _service.SaveDroitAsync(new Droit
        //     {
        //         Reference = "NewDroit",
        //         Status = DroitStatus.Received
        //     });
        //     
        //     await _service.SaveDroitAsync(new Droit
        //     {
        //         Reference = "NoMatch",
        //         Status = DroitStatus.Received
        //     });
        //
        //     var droitSearchForm = new DroitSearchForm()
        //     {
        //         Reference = "NewDroit"
        //     };
        //
        //     // Act
        //
        //     var results =
        //         await _service.AdvancedSearchDroitsAsync(droitSearchForm, new SearchOptions());
        //
        //     // Assert
        //     Assert.NotEmpty(results.Items);
        //     Assert.Equal(1, results.TotalCount);
        // }
        
    }
}
