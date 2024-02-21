using AutoMapper;
using Droits.Models;
using Droits.Models.Entities;
using Droits.Services;
using Microsoft.Extensions.Logging;

namespace Droits.Tests.UnitTests.Services
{
    public class MigrationServiceUnitTests
    {
        private readonly MigrationService _service;
        private readonly Mock<IDroitService> _mockDroitService;
        private readonly Mock<ILogger<MigrationService>> _mockLogger;

        public MigrationServiceUnitTests()
        {
            _mockDroitService = new Mock<IDroitService>();
            var mockFileService = new Mock<IDroitFileService>();
            var mockWreckMaterialService = new Mock<IWreckMaterialService>();
            var mockSalvorService = new Mock<ISalvorService>();
            var mockImageService = new Mock<IImageService>();
            var mockWreckService = new Mock<IWreckService>();
            var mockNoteService = new Mock<INoteService>();
           var mockUserService = new Mock<IUserService>();

            var mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<MigrationService>>();

            _service = new MigrationService(_mockLogger.Object,_mockDroitService.Object,
                mockWreckMaterialService.Object, mockSalvorService.Object, mockImageService.Object,
                mockFileService.Object,  mockWreckService.Object,mockNoteService.Object, mockUserService.Object ,mockMapper.Object);
        }


        [Fact]
        public async Task HandleTriageCsv_UpdatesGivenDroits()
        {
            // Assemble
            var records = new List<TriageRowDto>()
            {
                new()
                {
                    DroitReference = "001/24",
                    TriageNumber = "3"
                },
                new()
                {
                    DroitReference = "002/24",
                    TriageNumber = "5"
                },
            };

            var droitOne = new Droit() { Id = Guid.NewGuid(), Reference = "001/24" };
            var droitTwo = new Droit() { Id = Guid.NewGuid(), Reference = "002/24" };

            _mockDroitService.Setup(ds => ds.GetDroitByReferenceAsync(droitOne.Reference))
                .ReturnsAsync(droitOne);
            _mockDroitService.Setup(ds => ds.GetDroitByReferenceAsync(droitTwo.Reference))
                .ReturnsAsync(droitTwo);
            _mockDroitService.Setup(ds => ds.SaveDroitAsync(droitOne)).ReturnsAsync(droitOne);
            _mockDroitService.Setup(ds => ds.SaveDroitAsync(droitTwo)).ReturnsAsync(droitTwo);

            // Apply

            await _service.HandleTriageCsvAsync(records);
            // Assert
            
            _mockDroitService.Verify(r => r.GetDroitByReferenceAsync(droitOne.Reference), Times.Once);
            _mockDroitService.Verify(r => r.GetDroitByReferenceAsync(droitTwo.Reference), Times.Once);
            _mockDroitService.Verify(r => r.SaveDroitAsync(It.IsAny<Droit>()), Times.Exactly(2));
            
        }
        
    }
    
    
}