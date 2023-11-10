using Droits.Models.DTOs;
using Droits.Models.Entities;
using Droits.Models.Enums;

namespace Droits.Tests.UnitTests.Model.DTOs;

public class WreckExportDtoTests
{
    public WreckExportDtoTests()
    {
        
    }


    [Fact]
    public void WreckDto_WithAGivenWreck_ReturnsCorrectInformation()
    {
        // Assemble
        var wreck = new Wreck()
        {
            Id = Guid.NewGuid(),
            Name = "Boat",
            VesselConstructionDetails = "Hammer",
            VesselYearConstructed = 1995,
            DateOfLoss = new DateTime(),
            InUkWaters = true,
            IsWarWreck = true,
            IsAnAircraft = true,
            Latitude = 20,
            Longitude = 23,
            IsProtectedSite = false,
            ProtectionLegislation = "MCA",
            OwnerName = "John Doe",
            OwnerEmail = "RayMe@FarSo.com",
            OwnerNumber = "09876543210",
            AdditionalInformation = "foo"
        };
        // Act
        var wreckDto = new WreckExportDto(wreck);
        // Assert
        Assert.Equal(wreck.Name,wreckDto.Name);
        Assert.Equal(wreck.VesselConstructionDetails,wreckDto.VesselConstructionDetails);
        Assert.Equal(wreck.VesselYearConstructed,wreckDto.VesselYearConstructed);
        Assert.Equal(wreck.DateOfLoss,wreckDto.DateOfLoss);
        Assert.Equal(wreck.InUkWaters,wreckDto.InUkWaters);
        Assert.Equal(wreck.IsWarWreck,wreckDto.IsWarWreck);
        Assert.Equal(wreck.IsAnAircraft,wreckDto.IsAnAircraft);
        Assert.Equal(wreck.Latitude,wreckDto.Latitude);
        Assert.Equal(wreck.Longitude,wreckDto.Longitude);
        Assert.Equal(wreck.IsProtectedSite,wreckDto.IsProtectedSite);
        Assert.Equal(wreck.ProtectionLegislation,wreckDto.ProtectionLegislation);
        Assert.Equal(wreck.OwnerName,wreckDto.OwnerName);
        Assert.Equal(wreck.OwnerEmail,wreckDto.OwnerEmail);
        Assert.Equal(wreck.AdditionalInformation,wreckDto.AdditionalInformation);
    }
}