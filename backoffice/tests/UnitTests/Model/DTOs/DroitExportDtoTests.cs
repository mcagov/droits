using Droits.Models.DTOs;
using Droits.Models.Entities;
using Droits.Models.Enums;

namespace Droits.Tests.UnitTests.Model.DTOs;

public class DroitExportDtoTests
{
    public DroitExportDtoTests()
    {
        
    }


    [Fact]
    public void DroitDto_WithAGivenDroit_ReturnsCorrectInformation()
    {
        // Assemble
        var wreck = new Wreck()
        {
            Id = Guid.NewGuid(),
            Name = "Boat"
        };
        var salvor = new Salvor()
        {
            Id = Guid.NewGuid(),
            Name = "Neptune"
        };
        var user = new ApplicationUser()
        {
            Id = Guid.NewGuid(),
            Name = "Sinbad"
        };
        var droit = new Droit()
        {
            Id = Guid.NewGuid(),
            Reference = "Ref1",
            Wreck = wreck,
            Salvor = salvor,
            AssignedToUser = user,
            Status = DroitStatus.Received
        };
        // Act
        var droitDto = new DroitExportDto(droit);
        // Assert
        Assert.Equal(droit.Id,droitDto.Id);
        Assert.Equal(droit.Reference,droitDto.Reference);
        Assert.Equal(droit.Status.ToString(),droitDto.Status);
        Assert.Equal(wreck.Name,droitDto.WreckName);
        Assert.Equal(salvor.Name,droitDto.SalvorName);
        Assert.Equal(user.Name,droitDto.AssignedTo);
    }
}