using AutoMapper;
using Droits.Data.Mappers.Imports;
using Droits.Models.DTOs.Imports;
using Droits.Models.FormModels;

namespace Droits.Tests.UnitTests.Data.Mappers.Import;

public class WreckMaterialRowDtoMappingProfileUnitTests
{
    private readonly IMapper _mapper;
    public WreckMaterialRowDtoMappingProfileUnitTests()
    {
        var profile = new WreckMaterialRowDtoMappingProfile();
        var configuration = new MapperConfiguration(cfg =>
            {
            cfg.AddProfile(profile);

            }
        );
    
        _mapper = new Mapper(configuration);
    }


    [Fact]
    public void TestMapping_WMRowDtoToWreckMaterialForm()
    {
        
        // Assemble
        
        var rowDto = new WMRowDto()
        {
            Name = "Test",
            Description = "This is a test",
            Quantity = "1",
            SalvorValuation = "2",
            ReceiverValuation = "3",
            ValueConfirmed = "yes",
            StorageLine1 = "foo",
            StorageLine2 = "bar",
            StorageCityTown = "fooville",
            StorageCounty = "barshire",
            StoragePostcode = "f008ar"
        };
        
        // Act
        
        var wreckMaterialForm = _mapper.Map<WreckMaterialForm>(rowDto);
        
        // Assert
        
        Assert.Equal("Test",wreckMaterialForm.Name);
        Assert.Equal("This is a test",wreckMaterialForm.Description);
        Assert.Equal(1,wreckMaterialForm.Quantity);
        Assert.Equal(2,wreckMaterialForm.SalvorValuation);
        Assert.Equal(3,wreckMaterialForm.ReceiverValuation);
        Assert.True(wreckMaterialForm.ValueConfirmed);
        Assert.Equal("foo",wreckMaterialForm.StorageAddress?.Line1);
        Assert.Equal("bar",wreckMaterialForm.StorageAddress?.Line2);
        Assert.Equal("fooville",wreckMaterialForm.StorageAddress?.Town);
        Assert.Equal("barshire",wreckMaterialForm.StorageAddress?.County);
        Assert.Equal("f008ar",wreckMaterialForm.StorageAddress?.Postcode);
        
    }
    
    [Fact]
    public void TestMappingWithMissingFields_WMRowDtoToWreckMaterialForm_ReturnsCorrectValues()
    {
        
        // Assemble
        
        var rowDto = new WMRowDto()
        {
            Name = "Test",
            Description = "This is a test",
            Quantity = "1",
            SalvorValuation = "2",
            ReceiverValuation = null,
            ValueConfirmed = "no",
            StorageLine1 = null,
            StorageLine2 = null,
            StorageCityTown = "fooville",
            StorageCounty = "barshire",
            StoragePostcode = "f008ar"
        };
        
        // Act
        
        var wreckMaterialForm = _mapper.Map<WreckMaterialForm>(rowDto);
        
        // Assert
        
        Assert.Equal("Test",wreckMaterialForm.Name);
        Assert.Equal("This is a test",wreckMaterialForm.Description);
        Assert.Equal(1,wreckMaterialForm.Quantity);
        Assert.Equal(2,wreckMaterialForm.SalvorValuation);
        Assert.Null(wreckMaterialForm.ReceiverValuation);
        Assert.False(wreckMaterialForm.ValueConfirmed);
        Assert.Equal("",wreckMaterialForm.StorageAddress?.Line1);
        Assert.Equal("",wreckMaterialForm.StorageAddress?.Line2);
        Assert.Equal("fooville",wreckMaterialForm.StorageAddress?.Town);
        Assert.Equal("barshire",wreckMaterialForm.StorageAddress?.County);
        Assert.Equal("f008ar",wreckMaterialForm.StorageAddress?.Postcode);

    }
    
    [Fact]
    public void TestMappingWithEmptyFields_WMRowDtoToWreckMaterialForm_ReturnsCorrectValues()
    {
        
        // Assemble
        
        var rowDto = new WMRowDto()
        {
            Name = "Test",
            Description = "This is a test",
            Quantity = "1",
            SalvorValuation = "2",
            ReceiverValuation = "",
            ValueConfirmed = "no",
            StorageLine1 = "",
            StorageLine2 = "",
            StorageCityTown = "fooville",
            StorageCounty = "barshire",
            StoragePostcode = "f008ar"
        };
        
        // Act
        
        var wreckMaterialForm = _mapper.Map<WreckMaterialForm>(rowDto);
        
        // Assert
        
        Assert.Equal("Test",wreckMaterialForm.Name);
        Assert.Equal("This is a test",wreckMaterialForm.Description);
        Assert.Equal(1,wreckMaterialForm.Quantity);
        Assert.Equal(2,wreckMaterialForm.SalvorValuation);
        Assert.Null(wreckMaterialForm.ReceiverValuation);
        Assert.False(wreckMaterialForm.ValueConfirmed);
        Assert.Equal("",wreckMaterialForm.StorageAddress?.Line1);
        Assert.Equal("",wreckMaterialForm.StorageAddress?.Line2);
        Assert.Equal("fooville",wreckMaterialForm.StorageAddress?.Town);
        Assert.Equal("barshire",wreckMaterialForm.StorageAddress?.County);
        Assert.Equal("f008ar",wreckMaterialForm.StorageAddress?.Postcode);
        

    }
}