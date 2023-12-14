using AutoMapper;
using Droits.Models.DTOs.Powerapps;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Data.Mappers.Powerapps;

namespace Droits.Tests.UnitTests.Data.Mappers.Powerapps
{
    public class PowerAppsWreckMappingProfileUnitTests
    {
        private readonly IMapper _mapper;

        public PowerAppsWreckMappingProfileUnitTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PowerAppsWreckMappingProfile());
            });
            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void TestMapping_PowerappsWreckDtoToWreck_WithHistoricType()
        {
            // Arrange
            var powerappsWreckDto = new PowerappsWreckDto
            {
                Mcawrecksid = "1",
                Name = "Test Wreck",
                WreckType = 614880000, // Historic Type
                IsAircraft = true,
                ProtectedSite = true,
                IsWarWreck = true,
                DateOfLoss = DateTime.Now.AddDays(-100),
                ProtectionLegislationValue = "6b097d27-c525-ec11-b6e6-000d3ad65574",
                Longitude = 123.456,
                Latitude = 78.90,
                WreckOwner = new PowerappsWreckOwnerDto
                {
                    FullName = "Owner Full Name",
                    EmailAddress = "owner@example.com",
                    AddressComposite = "Owner Address",
                    MobilePhone = "123-456-7890"
                }
            };

            // Act
            var wreck = _mapper.Map<Wreck>(powerappsWreckDto);

            // Assert
            Assert.Equal("1", wreck.PowerappsWreckId);
            Assert.Equal(WreckType.Historic, wreck.WreckType);
            Assert.Equal("Test Wreck", wreck.Name);
            Assert.True(wreck.IsAnAircraft);
            Assert.True(wreck.IsProtectedSite);
            Assert.True(wreck.IsWarWreck);
            Assert.Equal(powerappsWreckDto.DateOfLoss, wreck.DateOfLoss);
            Assert.Equal("Historic Monuments and Archaeological Objects (Northern Ireland) Order 1995", wreck.ProtectionLegislation);
            Assert.Equal(123.456, wreck.Longitude);
            Assert.Equal(78.90, wreck.Latitude);
            Assert.Equal("Owner Full Name", wreck.OwnerName);
            Assert.Equal("owner@example.com", wreck.OwnerEmail);
            Assert.Equal("Owner Address", wreck.OwnerAddress);
            Assert.Equal("123-456-7890", wreck.OwnerNumber);
            Assert.False(wreck.InUkWaters);
            Assert.Equal("",wreck.AdditionalInformation);
            Assert.Equal("",wreck.ConstructionDetails);
            Assert.Null(wreck.YearConstructed);

        }


        [Fact]
        public void TestMapping_PowerappsWreckDtoToWreck_WithModernType()
        {
            // Arrange
            var powerappsWreckDto = new PowerappsWreckDto
            {
                Mcawrecksid = "1",
                Name = "Test Modern Wreck",
                WreckType = 614880001, // Modern Type
                IsAircraft = false,
                ProtectedSite = false,
                IsWarWreck = false,
                DateOfLoss = DateTime.Now.AddDays(-100),
                ProtectionLegislationValue = "cec2eab2-ac85-ec11-8d21-00224842d40e",
                Longitude = 123.456,
                Latitude = 78.90,
                WreckOwner = new PowerappsWreckOwnerDto
                {
                    FullName = "Owner Full Name",
                    EmailAddress = "owner@example.com",
                    AddressComposite = "Owner Address",
                    MobilePhone = "123-456-7890"
                }
            };

            // Act
            var wreck = _mapper.Map<Wreck>(powerappsWreckDto);

            // Assert
            Assert.Equal("1", wreck.PowerappsWreckId);
            Assert.Equal(WreckType.Modern, wreck.WreckType);
            Assert.Equal("Test Modern Wreck", wreck.Name);
            Assert.False(wreck.IsAnAircraft);
            Assert.False(wreck.IsProtectedSite);
            Assert.False(wreck.IsWarWreck);
            Assert.Equal(powerappsWreckDto.DateOfLoss, wreck.DateOfLoss);
            Assert.Equal("Protection of Wrecks Act 1973", wreck.ProtectionLegislation);
            Assert.Equal(123.456, wreck.Longitude);
            Assert.Equal(78.90, wreck.Latitude);
            Assert.Equal("Owner Full Name", wreck.OwnerName);
            Assert.Equal("owner@example.com", wreck.OwnerEmail);
            Assert.Equal("Owner Address", wreck.OwnerAddress);
            Assert.Equal("123-456-7890", wreck.OwnerNumber);
            Assert.False(wreck.InUkWaters);
            Assert.Equal("",wreck.AdditionalInformation);
            Assert.Equal("",wreck.ConstructionDetails);
            Assert.Null(wreck.YearConstructed);
            
        }

        [Fact]
        public void TestMapping_PowerappsWreckDtoToWreck_WithAllNullFields()
        {
            // Arrange
            var powerappsWreckDto = new PowerappsWreckDto
            {
                Mcawrecksid = null,
                Name = null,
                WreckType = null,
                IsAircraft = null,
                ProtectedSite = null,
                IsWarWreck = null,
                DateOfLoss = null,
                ProtectionLegislationValue = null,
                Longitude = null,
                Latitude = null,
                WreckOwner = null
            };

            // Act
            var wreck = _mapper.Map<Wreck>(powerappsWreckDto);

            // Assert
            Assert.Null(wreck.PowerappsWreckId);
            Assert.Null(wreck.WreckType);
            Assert.Null(wreck.Name);
            Assert.False(wreck.IsAnAircraft);
            Assert.False(wreck.IsProtectedSite);
            Assert.False(wreck.IsWarWreck);
            Assert.Null(wreck.DateOfLoss);
            Assert.Null(wreck.ProtectionLegislation);
            Assert.Null(wreck.Longitude);
            Assert.Null(wreck.Latitude);
            Assert.Equal("",wreck.OwnerName);
            Assert.Equal("",wreck.OwnerEmail);
            Assert.Equal("",wreck.OwnerAddress);
            Assert.Equal("",wreck.OwnerNumber);
            Assert.False(wreck.InUkWaters);
            Assert.Equal("",wreck.AdditionalInformation);
            Assert.Equal("",wreck.ConstructionDetails);
            Assert.Null(wreck.YearConstructed);
        }

    }
}
