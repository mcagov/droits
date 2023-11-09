using AutoMapper;
using Droits.Data.Mappers;
using Droits.Models.DTOs;
using Droits.Models.Entities;

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
        public void TestMapping_SubmittedReportDtoToDroit()
        {
            // Arrange
            var submittedReportDto = new SubmittedReportDto
            {
                LocationDescription = "Sample Location"
            };

            // Act
            var droit = _mapper.Map<Droit>(submittedReportDto);

            // Assert
            Assert.Equal("Sample Location", droit.LocationDescription);
        }

        [Fact]
        public void TestMapping_SubmittedReportDtoWithNullValues()
        {
            // Arrange
            var submittedReportDto = new SubmittedReportDto
            {
                LocationDescription = null
            };

            // Act
            var droit = _mapper.Map<Droit>(submittedReportDto);

            // Assert
            Assert.Equal(string.Empty, droit.LocationDescription);
        }
    }
}