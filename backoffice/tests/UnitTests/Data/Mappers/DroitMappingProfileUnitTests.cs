using AutoMapper;
using Droits.Data.Mappers;
using Droits.Models.DTOs;
using Droits.Models.Entities;
using Droits.Models.Enums;

namespace Droits.Tests.UnitTests.Data.Mappers
{
    public class DroitMappingProfileTests
    {
        private readonly IMapper _mapper;

        public DroitMappingProfileTests()
        {
            var profile = new DroitMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            _mapper = new Mapper(configuration);
        }


        [Fact]
        public void TestDroitMapping_SubmittedReportDtoWithNullValues()
        {
            // Arrange
            var submittedReportDto = new SubmittedReportDto
            {
                ReportDate = null,
                WreckFindDate = null,
                Latitude = null,
                Longitude = null,
                LocationRadius = null,
                LocationDescription = null,
                VesselDepth = null,
                RemovedFrom = null,
                ClaimSalvage = null,
                SalvageServices = null
            };

            // Act
            var droit = _mapper.Map<Droit>(submittedReportDto);

            // Assert
            Assert.Null(droit.Latitude);
            Assert.Null(droit.Longitude);
            Assert.Null(droit.LocationRadius);
            Assert.Equal("",droit.LocationDescription);
            Assert.Null(droit.RecoveredFrom);
            Assert.Null(droit.Depth);
            Assert.Null(droit.RecoveredFromLegacy);
            Assert.False(droit.SalvageAwardClaimed);
            Assert.Null(droit.ServicesDescription);
            Assert.Empty(droit.WreckMaterials);
        }


        [Fact]
        public void TestDroitMapping_SubmittedReportDtoWithValidValues()
        {
            // Arrange
            var submittedReportDto = new SubmittedReportDto
            {
                ReportDate = "2023-01-01",
                WreckFindDate = "2022-01-01",
                Latitude = 41.7125, 
                Longitude = -49.9469, 
                LocationRadius = 50, 
                LocationDescription = "Some place in water",
                VesselDepth = 50d,
                RemovedFrom = "afloat",
                ClaimSalvage = "true", 
                SalvageServices = "Salvage services description",
                VesselConstructionYear = ""
            };

            // Act
            var droit = _mapper.Map<Droit>(submittedReportDto);

            // Assert
            Assert.Equal(new DateTime(2023,01,01), droit.ReportedDate);
            Assert.Equal(new DateTime(2022,01,01), droit.DateFound);
            Assert.Equal(submittedReportDto.Latitude, droit.Latitude);
            Assert.Equal(50, droit.Depth);
            Assert.Equal(submittedReportDto.Longitude, droit.Longitude);
            Assert.Equal(submittedReportDto.LocationRadius, droit.LocationRadius);
            Assert.Equal(submittedReportDto.LocationDescription, droit.LocationDescription);
            Assert.Equal(RecoveredFrom.Afloat, droit.RecoveredFrom); 
            Assert.Equal(submittedReportDto.RemovedFrom, droit.RecoveredFromLegacy);
            Assert.True(droit.SalvageAwardClaimed);
            Assert.Equal(submittedReportDto.SalvageServices, droit.ServicesDescription);
            Assert.Empty(droit.WreckMaterials);
        }
        
        
        [Fact]
        public void TestDroitMapping_SubmittedReportDtoWithValidValues_ParsedDoubleToInt()
        {
            // Arrange
            var submittedReportDto = new SubmittedReportDto
            {
                ReportDate = "2023-01-01",
                WreckFindDate = "2022-01-01",
                VesselDepth = 50.2,
            };

            // Act
            var droit = _mapper.Map<Droit>(submittedReportDto);

            // Assert
            Assert.Equal(new DateTime(2023,01,01), droit.ReportedDate);
            Assert.Equal(new DateTime(2022,01,01), droit.DateFound);
            Assert.Equal(50, droit.Depth);
        }
        
        
        [Theory]
        [InlineData("afloat", RecoveredFrom.Afloat)]
        [InlineData("sea shore", RecoveredFrom.SeaShore)]
        [InlineData("seabed", RecoveredFrom.Seabed)]
        [InlineData("a float", RecoveredFrom.Afloat)]
        [InlineData("sea bed", RecoveredFrom.Seabed)]
        [InlineData("SeaShore", RecoveredFrom.SeaShore)]
        public void TestMapping_RemovedFromValues(string removedFromValue, RecoveredFrom expectedEnumValue)
        {
            // Arrange
            var submittedReportDto = new SubmittedReportDto
            {
                RemovedFrom = removedFromValue
            };

            // Act
            var droit = _mapper.Map<Droit>(submittedReportDto);

            // Assert
            Assert.Equal(expectedEnumValue, droit.RecoveredFrom);
            Assert.Equal(removedFromValue, droit.RecoveredFromLegacy);
        }
    }
}