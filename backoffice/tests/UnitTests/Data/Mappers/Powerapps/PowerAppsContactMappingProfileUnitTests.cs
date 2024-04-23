using AutoMapper;
using Droits.Models.DTOs.Powerapps;
using Droits.Models.Entities;
using Droits.Data.Mappers.Powerapps;

namespace Droits.Tests.UnitTests.Data.Mappers.Powerapps;

public class PowerAppsContactMappingProfileUnitTests
{
    private readonly IMapper _mapper;

    public PowerAppsContactMappingProfileUnitTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new PowerAppsContactMappingProfile());
        });
        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public void TestMapping_PowerappsContactDtoToSalvor_Full_WithMultiple()
    {
        // Arrange
        var powerappsContactDto = new PowerappsContactDto()
        {
            ContactId = "111",
            FullName = "John Doe",
            EmailAddress = "johndoe@example.com",
            Telephone1 = "123-456-7890",
            Telephone2 = "987-654-3210",
            Telephone3 = "555-555-5555",
            MobilePhone = "999-999-9999",
            AddressLine1 = "456 Oak Street",
            AddressLine2 = "Suite 200",
            AddressLine3 = "Building XYZ",
            AddressCity = "Anytown",
            AddressCounty = "Anycounty",
            AddressCountry = "Country",
            AddressPostalCode = "12345",
            AddressComposite = "456 Oak Street\r\nSuite 200\r\nBuilding XYZ\r\nAnytown 12345"
        };

        // Mapping
        var salvor = _mapper.Map<Salvor>(powerappsContactDto);

        // Assert
        Assert.Equal("111", salvor.PowerappsContactId);
        Assert.Equal(powerappsContactDto.FullName, salvor.Name);
        Assert.Equal(powerappsContactDto.EmailAddress, salvor.Email);
        Assert.Equal($"{powerappsContactDto.Telephone1} / {powerappsContactDto.Telephone2} / {powerappsContactDto.Telephone3}", salvor.TelephoneNumber);
        Assert.Equal(powerappsContactDto.MobilePhone, salvor.MobileNumber);
        Assert.Equal(powerappsContactDto.AddressLine1, salvor.Address.Line1);
        Assert.Equal($"{powerappsContactDto.AddressLine2}, {powerappsContactDto.AddressLine3}", salvor.Address.Line2);
        Assert.Equal(powerappsContactDto.AddressCity, salvor.Address.Town);
        Assert.Equal(powerappsContactDto.AddressCounty, salvor.Address.County);
        Assert.Equal(powerappsContactDto.AddressPostalCode, salvor.Address.Postcode);

    }
        
        
    [Fact]
    public void TestMapping_PowerappsContactDtoToSalvor_Full_WithSingle()
    {
        // Arrange
        var powerappsContactDto = new PowerappsContactDto()
        {
            ContactId = "111",
            FullName = "John Doe",
            EmailAddress = "johndoe@example.com",
            Telephone1 = "123-456-7890",
            Telephone2 = null,
            Telephone3 = null,
            MobilePhone = "999-999-9999",
            AddressLine1 = "456 Oak Street",
            AddressLine2 = "Suite 200",
            AddressLine3 = null,
            AddressCity = "Anytown",
            AddressCounty = "Anycounty",
            AddressCountry = "Country",
            AddressPostalCode = "12345",
            AddressComposite = "456 Oak Street\r\nSuite 200\r\nBuilding XYZ\r\nAnytown 12345"
        };

        // Mapping
        var salvor = _mapper.Map<Salvor>(powerappsContactDto);

        // Assert
        Assert.Equal("111", salvor.PowerappsContactId);
        Assert.Equal(powerappsContactDto.FullName, salvor.Name);
        Assert.Equal(powerappsContactDto.EmailAddress, salvor.Email);
        Assert.Equal(powerappsContactDto.Telephone1, salvor.TelephoneNumber);
        Assert.Equal(powerappsContactDto.MobilePhone, salvor.MobileNumber);
        Assert.Equal(powerappsContactDto.AddressLine1, salvor.Address.Line1);
        Assert.Equal(powerappsContactDto.AddressLine2, salvor.Address.Line2);
        Assert.Equal(powerappsContactDto.AddressCity, salvor.Address.Town);
        Assert.Equal(powerappsContactDto.AddressCounty, salvor.Address.County);
        Assert.Equal(powerappsContactDto.AddressPostalCode, salvor.Address.Postcode);

    }
}