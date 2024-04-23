using System.Diagnostics;
using AutoMapper;
using Droits.Data.Mappers.Imports;
using Droits.Models.DTOs.Imports;
using Droits.Models.Entities;
using Droits.Models.Enums;

namespace Droits.Tests.UnitTests.Data.Mappers.Import;

public class AccessSalvorMappingProfileUnitTests
{
    private readonly IMapper _mapper;

    public AccessSalvorMappingProfileUnitTests()
    {
        var profile = new AccessSalvorMappingProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
        _mapper = new Mapper(configuration);
    }

    [Fact]
    public void TestMapping_AccessDtoToSalvor()
    {
        // Arrange
        var dto = new AccessDto()
        {
            SalvorName = "Test Name",
            Address = "foo,bar,baz,testington",
            PostCode = "TE57 1NG"
        };
        
        // Act
        var salvor = _mapper.Map<Salvor>(dto);

        // Assert
        Assert.Equal(dto.SalvorName, salvor.Name);
        Assert.Contains("@GeneratedSalvorEmail.com", salvor.Email);
        Assert.NotNull(salvor.Address);
        Assert.Equal("foo", salvor.Address.Line1);
        Assert.Equal("bar", salvor.Address.Line2);
        Assert.Equal("baz", salvor.Address.Town);
        Assert.Equal("testington", salvor.Address.County);
        Assert.Equal("TE57 1NG", salvor.Address.Postcode);
    }
    
    [Fact]
    public void TestMapping_AccessDtoToSalvorNullValues()
    {
        // Arrange
        var dto = new AccessDto()
        {
            SalvorName = null,
            Address = null,
            PostCode = null
        };
        
        // Act
        var salvor = _mapper.Map<Salvor>(dto);
        
        // Assert
        Assert.Equal(string.Empty, salvor.Name);
        Assert.Contains("@GeneratedSalvorEmail.com", salvor.Email);
        Assert.NotNull(salvor.Address);
        Assert.Equal(string.Empty, salvor.Address.Line1);
        Assert.Equal(string.Empty, salvor.Address.Line2);
        Assert.Equal(string.Empty, salvor.Address.Town);
        Assert.Equal(string.Empty, salvor.Address.County);
        Assert.Equal(string.Empty, salvor.Address.Postcode);
    }
    
    

}