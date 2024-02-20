using AutoMapper;
using Droits.Exceptions;
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
            var mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<MigrationService>>();

            _service = new MigrationService(_mockLogger.Object,_mockDroitService.Object,
                mockWreckMaterialService.Object, mockSalvorService.Object, mockImageService.Object,
                mockFileService.Object,  mockWreckService.Object,mockNoteService.Object, mockMapper.Object);
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

            _mockDroitService.Setup(ds => ds.GetDroitByReferenceAsync("001/24"))
                .ReturnsAsync(droitOne);
            _mockDroitService.Setup(ds => ds.GetDroitByReferenceAsync("002/24"))
                .ReturnsAsync(droitTwo);
            _mockDroitService.Setup(ds => ds.SaveDroitAsync(droitOne)).ReturnsAsync(droitOne);
            _mockDroitService.Setup(ds => ds.SaveDroitAsync(droitTwo)).ReturnsAsync(droitTwo);

            // Apply

            await _service.HandleTriageCsvAsync(records);
            // Assert
            
            _mockDroitService.Verify(r => r.GetDroitByReferenceAsync("001/24"), Times.Once);
            _mockDroitService.Verify(r => r.GetDroitByReferenceAsync("002/24"), Times.Once);
            _mockDroitService.Verify(r => r.SaveDroitAsync(droitOne), Times.Once);
            _mockDroitService.Verify(r => r.SaveDroitAsync(droitTwo), Times.Once);
            
        }
        
        [Fact]
        public async Task HandleTriageCsv_RaisesError_WhenGivenATriageNumberOutOfRange()
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
                    TriageNumber = "6"
                },
            };
            

            // Apply

            await _service.HandleTriageCsvAsync(records);
            // Assert

            
        }
        
    }
    
    
}