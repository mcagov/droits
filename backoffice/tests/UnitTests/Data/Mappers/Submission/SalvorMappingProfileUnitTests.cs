using AutoMapper;
using Droits.Data.Mappers.Submission;
using Droits.Models.DTOs;
using Droits.Models.Entities;

namespace Droits.Tests.UnitTests.Data.Mappers.Submission;

public class SalvorMappingProfileTests
{
    private readonly IMapper _mapper;

    public SalvorMappingProfileTests()
    {
        var profile = new SalvorMappingProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
        _mapper = new Mapper(configuration);
    }
        
    [Fact]
    public void TestMapping_SubmittedReportDtoToSalvor()
    {
        // Arrange
        var submittedReportDto = new SubmittedReportDto
        {
            Personal = new SubmittedPersonalDto
            {
                Email = "test@example.com",
                FullName = "John Doe",
                TelephoneNumber = "123-456-7890",
                AddressLine1 = "123 Main St",
                AddressLine2 = "Apt 4B",
                AddressTown = "Cityville",
                AddressCounty = "County",
                AddressPostcode = "12345"
            }
        };

        // Act
        var salvor = _mapper.Map<Salvor>(submittedReportDto);

        // Assert
        Assert.Equal("test@example.com", salvor.Email);
        Assert.Equal("John Doe", salvor.Name);
        Assert.Equal("123-456-7890", salvor.TelephoneNumber);
        Assert.Equal("123 Main St", salvor.Address.Line1);
        Assert.Equal("Apt 4B", salvor.Address.Line2);
        Assert.Equal("Cityville", salvor.Address.Town);
        Assert.Equal("County", salvor.Address.County);
        Assert.Equal("12345", salvor.Address.Postcode);
    }

    [Fact]
    public void TestMapping_SubmittedReportDtoWithNullValues()
    {
        // Arrange
        var submittedReportDto = new SubmittedReportDto
        {
            Personal = null
        };

        // Act
        var salvor = _mapper.Map<Salvor>(submittedReportDto);

        // Assert
        Assert.Equal(string.Empty, salvor.Email);
        Assert.Equal(string.Empty, salvor.Name);
        Assert.Equal(string.Empty, salvor.TelephoneNumber);
        Assert.Equal(string.Empty, salvor.Address.Line1);
        Assert.Equal(string.Empty, salvor.Address.Line2);
        Assert.Equal(string.Empty, salvor.Address.Town);
        Assert.Equal(string.Empty, salvor.Address.County);
        Assert.Equal(string.Empty, salvor.Address.Postcode);
    }
}