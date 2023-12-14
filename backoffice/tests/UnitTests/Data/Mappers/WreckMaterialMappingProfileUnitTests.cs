using AutoMapper;
using Droits.Data.Mappers;
using Droits.Models.DTOs;
using Droits.Models.Entities;

namespace Droits.Tests.UnitTests.Data.Mappers
{
    public class WreckMaterialMappingProfileTests
    {
        private readonly IMapper _mapper;

        public WreckMaterialMappingProfileTests()
        {
            var profile = new WreckMaterialMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            _mapper = new Mapper(configuration);
        }

        [Fact]
        public void TestMapping_SubmittedWreckMaterialDtoToWreckMaterial()
        {
            // Arrange
            var submittedWreckMaterialDto = new SubmittedWreckMaterialDto
            {
                Description = "Sample Material",
                Quantity = "10",
                Value = 99.99d,
                ValueKnown = "true",
                AddressDetails = new SubmittedAddressDetailsDto()
                {
                    AddressLine1 = "123 Main St",
                    AddressLine2 = "Apt 4B",
                    AddressTown = "Cityville",
                    AddressCounty = "County",
                    AddressPostcode = "12345"
                }
            };

            // Act
            var wreckMaterial = _mapper.Map<WreckMaterial>(submittedWreckMaterialDto);

            // Assert
            Assert.Equal("Sample Material", wreckMaterial.Name);
            Assert.Equal(10, wreckMaterial.Quantity);
            Assert.Equal(99.99d, wreckMaterial.Value);
            Assert.True(wreckMaterial.ValueKnown);
            Assert.Equal("123 Main St", wreckMaterial.StorageAddress.Line1);
            Assert.Equal("Apt 4B", wreckMaterial.StorageAddress.Line2);
            Assert.Equal("Cityville", wreckMaterial.StorageAddress.Town);
            Assert.Equal("County", wreckMaterial.StorageAddress.County);
            Assert.Equal("12345", wreckMaterial.StorageAddress.Postcode);
        }

        [Fact]
        public void TestMapping_SubmittedWreckMaterialDtoWithNullValues()
        {
            // Arrange
            var submittedWreckMaterialDto = new SubmittedWreckMaterialDto
            {
                Description = null,
                Quantity = null,
                Value = null,
                ValueKnown = null,
                AddressDetails = null
            };

            // Act
            var wreckMaterial = _mapper.Map<WreckMaterial>(submittedWreckMaterialDto);

            // Assert
            
            // Assert
            Assert.Equal(string.Empty, wreckMaterial.Name);
            Assert.Equal(0, wreckMaterial.Quantity);
            Assert.Equal(0.0d, wreckMaterial.Value);
            Assert.False(wreckMaterial.ValueKnown);
            Assert.Equal(string.Empty, wreckMaterial.StorageAddress.Line1);
            Assert.Equal(string.Empty, wreckMaterial.StorageAddress.Line2);
            Assert.Equal(string.Empty, wreckMaterial.StorageAddress.Town);
            Assert.Equal(string.Empty, wreckMaterial.StorageAddress.County);
            Assert.Equal(string.Empty, wreckMaterial.StorageAddress.Postcode);
        }
    }
}
