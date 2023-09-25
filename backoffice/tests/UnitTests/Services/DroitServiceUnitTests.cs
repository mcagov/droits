using System.Text;
using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Repositories;
using Droits.Services;
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
            _service = new DroitService(mockLogger.Object, _mockRepo.Object, mockWreckMaterialService.Object, mockCurrentUserService.Object);
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
            var newDroit = new Droit { Reference = "Ref3" };
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Droit>())).ReturnsAsync(newDroit);

            // When
            var result = await _service.SaveDroitAsync(newDroit);

            // Then
            Assert.NotNull(result);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Droit>()), Times.Once);
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
            
            _mockRepo.Verify(r => r.UpdateAsync(It.Is<Droit>(d => d.Id == droitId && d.Status == DroitStatus.Research)), Times.Once);
            Assert.Equal(DroitStatus.Research, existingDroit.Status);
        }


        [Fact]
        public async Task SaveDroitAsync_AddsDroit_WhenDroitIdIsDefault()
        {
            // Given
            var newDroit = new Droit { Id = default(Guid) };
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Droit>())).ReturnsAsync(newDroit);

            // When
            var result = await _service.SaveDroitAsync(newDroit);

            // Then
            _mockRepo.Verify(r => r.AddAsync(newDroit), Times.Once);
            Assert.Equal(newDroit, result);
        }

        [Fact]
        public async Task SaveDroitAsync_ExistingDroit_UpdatesDroit()
        {
            // Given
            var existingDroitId = Guid.NewGuid();
            var existingDroit = new Droit { Id = existingDroitId, Reference = "ExistingRef" };
            _mockRepo.Setup(r => r.GetDroitAsync(existingDroitId)).ReturnsAsync(existingDroit);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Droit>())).ReturnsAsync(existingDroit);
            _mockRepo.Setup(r => r.IsReferenceUnique(It.IsAny<Droit>())).ReturnsAsync(true);

            // When
            var result = await _service.SaveDroitAsync(existingDroit);

            // Then
            _mockRepo.Verify(r => r.UpdateAsync(existingDroit), Times.Once);
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
            var emptyList = new List<Droit>() { };

            // When & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.ExportDroitsAsync(emptyList));
        }
        
        [Fact]
        public async Task ExportDroitsAsync_ListOfDroits_ReturnsData()
        {
            // Given
            var droits = new List<Droit>()
            {
                new Droit() { Id = new Guid(), Reference = "Ref1" },
                new Droit() { Id = new Guid(), Reference = "Ref2" }
            };

            // When
            var data = await _service.ExportDroitsAsync(droits);
            
            // Assert
            Assert.NotEmpty(data);
        }
        
        [Fact]
        public async Task ExportDroitsAsync_ListOfDroits_ReturnsCorrectData()
        {
            // Given
            var droits = new List<Droit>()
            {
                new Droit() { Id = new Guid(), Reference = "Ref3" },
                new Droit() { Id = new Guid(), Reference = "Ref4" }
            };

            // When
            var data = await _service.ExportDroitsAsync(droits);
            var dataString = Encoding.Default.GetString(data);
            var rows = dataString.Split("\r\n");
            
            var expectedHeaders = new List<string>{ "Id", "Reference", "Created", "LastModified", "WreckName", "WreckId", "SalvorName", "SalvorId", "AssignedTo", "Status" };
            
            
            // Assert
            var referenceIndex = expectedHeaders.IndexOf("Reference");
            
            var index = 0;
            foreach (var row in rows.Skip(1).Where(r => !string.IsNullOrWhiteSpace(r))) 
            {
                var cols = row.Split(",").ToList();
                
                Assert.True(cols.Count > referenceIndex);
                var reference = cols[referenceIndex];
                Assert.Equal(droits[index].Reference, reference);

                index++;
            }
            
            
        }
        
        //1. Expected headers. 
        //2. Multiple droits, expected output for each column. 
        
        [Fact]
        public async Task ExportDroitsAsync_ListOfDroits_ReturnsCorrecHeaders()
        {
            // Given
            var droits = new List<Droit>()
            {
                new Droit() { Id = new Guid(), Reference = "Ref3" },
                new Droit() { Id = new Guid(), Reference = "Ref4" }
            };

            var expectedHeaders = new List<string>{ "Id", "Reference", "Created", "LastModified", "WreckName", "WreckId", "SalvorName", "SalvorId", "AssignedTo", "Status" };
            
            // When
            var data = await _service.ExportDroitsAsync(droits);
            var dataString = Encoding.Default.GetString(data);

            var headers = dataString.Split("\r\n").First().Split(",").ToList();
            

            
            // Assert
            Assert.Equal(expectedHeaders, headers);
        }
        
        

        
        
        
    }
}
