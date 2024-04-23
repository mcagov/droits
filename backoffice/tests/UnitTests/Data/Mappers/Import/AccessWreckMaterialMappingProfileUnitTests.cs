using AutoMapper;
using Droits.Data.Mappers.Imports;
using Droits.Models.DTOs.Imports;
using Droits.Models.Entities;
using Droits.Models.Enums;

namespace Droits.Tests.UnitTests.Data.Mappers.Import;

public class AccessWreckMaterialMappingProfileTests
{
    private readonly IMapper _mapper;

    public AccessWreckMaterialMappingProfileTests()
    {
        var profile = new AccessWreckMaterialMappingProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
        _mapper = new Mapper(configuration);
    }

    [Fact]
    public void TestMapping_AccessDtoToWreckMaterial()
    {
        // Arrange
        var dto = new AccessDto()
        {
            DroitNumber = "123/456",
            Value = "1",
            Purchaser = "foo",
            Outcome = "bar",
            WhereSecured = "foo,bar,baz",
        
        };
            
        // Act
        var wreckMaterial = _mapper.Map<WreckMaterial>(dto);

        // Assert
        Assert.Contains("123/456", wreckMaterial.Name);
        Assert.Equal(1, wreckMaterial.SalvorValuation);
        Assert.Equal("foo",wreckMaterial.Purchaser);
        Assert.Equal(WreckMaterialOutcome.Other,wreckMaterial.Outcome);
        Assert.Equal("foo", wreckMaterial.StorageAddress.Line1);
        Assert.Equal("bar", wreckMaterial.StorageAddress.Line2);
        Assert.Equal("baz", wreckMaterial.StorageAddress.Town);
        Assert.Equal("", wreckMaterial.StorageAddress.County);
        Assert.Equal("", wreckMaterial.StorageAddress.Postcode);
    }
        
    [Fact]
    public void TestMapping_DifficultAccessDtoToWreckMaterial()
    {
        // Arrange
        var dto = new AccessDto()
        {
            DroitNumber = "123/456",
            Value = "123.654",
            Purchaser = "foo",
            Outcome = "Donated to Museum",
            WhereSecured = "foo,bar,baz,booey,f008ar,wut",
        
        };
            
        // Act
        var wreckMaterial = _mapper.Map<WreckMaterial>(dto);

        // Assert
        Assert.Contains("123/456", wreckMaterial.Name);
        Assert.Equal(123.654d, wreckMaterial.SalvorValuation);
        Assert.Equal("foo",wreckMaterial.Purchaser);
        Assert.Equal(WreckMaterialOutcome.DonatedToMuseum,wreckMaterial.Outcome);
        Assert.Equal("foo", wreckMaterial.StorageAddress.Line1);
        Assert.Equal("bar", wreckMaterial.StorageAddress.Line2);
        Assert.Equal("baz", wreckMaterial.StorageAddress.Town);
        Assert.Equal("booey", wreckMaterial.StorageAddress.County);
        Assert.Equal("f008ar", wreckMaterial.StorageAddress.Postcode);
    }

       
}