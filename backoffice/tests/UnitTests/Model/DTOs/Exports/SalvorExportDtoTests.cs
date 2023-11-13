using Droits.Models.DTOs;
using Droits.Models.DTOs.Exports;
using Droits.Models.Entities;
using Droits.Models.Enums;

namespace Droits.Tests.UnitTests.Model.DTOs;

public class SalvorExportDtoTests
{
    public SalvorExportDtoTests()
    {
        
    }


    [Fact]
    public void SalvorDto_WithAGivenSalvor_ReturnsCorrectInformation()
    {
        // Assemble
        var address = new Address()
        {
            Line1 = "1 Test Street",
            Line2 = "Test Estate",
            Town = "Teston",
            County = "TestShire",
            Postcode = "TE57 0NE"
        };
        var salvor = new Salvor()
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            TelephoneNumber = "12345678901",
            Email = "Test@TestMail.com",
            Address = address
            
        };
        // Act
        var salvorDto = new SalvorExportDto(salvor);
        // Assert
        Assert.Equal(salvor.Name,salvorDto.Name);
        Assert.Equal(salvor.TelephoneNumber,salvorDto.TelephoneNumber);
        Assert.Equal(salvor.Email,salvorDto.Email);
        Assert.Equal(address.Line1,salvorDto.AddressLine1);
        Assert.Equal(address.Line2,salvorDto.AddressLine2);
        Assert.Equal(address.Town,salvorDto.AddressTown);
        Assert.Equal(address.County,salvorDto.AddressCounty);
        Assert.Equal(address.Postcode,salvorDto.AddressPostcode);
    }
}