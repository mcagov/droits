using AutoMapper;
using Droits.Exceptions;
using Droits.Models.DTOs;
using Droits.Services;
using Microsoft.Extensions.Logging;

namespace Droits.Tests.UnitTests.Services
{
    public class ApiServiceUnitTests
    {
        
        private readonly ApiService _service;

        public ApiServiceUnitTests()
        {            
            var loggerMock = new Mock<ILogger<ApiService>>();
            var droitServiceMock = new Mock<IDroitService>();
            var wreckMaterialServiceMock = new Mock<IWreckMaterialService>();

            var salvorServiceMock = new Mock<ISalvorService>();
            var letterServiceMock = new Mock<ILetterService>();
            
            var mapperMock = new Mock<IMapper>();

            _service = new ApiService(loggerMock.Object, droitServiceMock.Object, wreckMaterialServiceMock.Object, salvorServiceMock.Object, letterServiceMock.Object, mapperMock.Object);
        }
        
        [Fact]
        public async Task SaveDroitReportAsync_NullReport_ThrowsDroitNotFoundException()
        {
            // Act and Assert
            await Assert.ThrowsAsync<DroitNotFoundException>(() => _service.SaveDroitReportAsync(null!));
        }

        [Fact]
        public void SaveDroitReportAsync_ValidReport_GeneratesReference()
        {
            // Arrange

            var report = new SubmittedReportDto
            {
                ReportDate = "2023-03-20",
                WreckFindDate = "2022-01-01",
                Latitude = 51.45399,
                Longitude = -3.17463,
                LocationRadius = 492,
                LocationDescription = "No additional info",
                VesselName = "",
                VesselConstructionYear = "",
                VesselSunkYear = "",
                VesselDepth = null,
                RemovedFrom = "afloat",
                WreckDescription = "",
                ClaimSalvage = "no",
                SalvageServices = "",
                Personal = new SubmittedPersonalDto
                {
                    FullName = "Test Salvor",
                    Email = "test.salvor@madetech.com",
                    TelephoneNumber = "07791351955",
                    AddressLine1 = "19 Test Close",
                    AddressLine2 = "Testing",
                    AddressTown = "Testington",
                    AddressCounty = "South Testington",
                    AddressPostcode = "TE571NG"
                },
                WreckMaterials = new List<SubmittedWreckMaterialDto>
                {
                    new()
                    {
                        Description = "empty bag",
                        Quantity = "1",
                        Value = 0.10d,
                        ValueKnown = "yes",
                        Image = null,
                        AddressDetails = new SubmittedAddressDetailsDto
                        {
                            AddressLine1 = "19 Test Close",
                            AddressLine2 = "Testing",
                            AddressTown = "Testington",
                            AddressCounty = "South Testington",
                            AddressPostcode = "TE571NG"
                        },
                        StorageAddress = "personal"
                    }
                }
            };
        }
    }
}