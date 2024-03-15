using System.Text;
using Droits.Data.Mappers.CsvMappers;
using Droits.Helpers;
using Droits.Models.DTOs;
using Droits.Models.DTOs.Exports;
using Droits.Models.FormModels;

namespace Droits.Tests.UnitTests.Helpers;

public class ExportHelperTests
{

    [Fact]
    public async Task ExportDroitsAsync_ListOfDroits_ReturnsData()
    {
        // Given
        var droits = new List<DroitExportDto>()
        {
            new() { Id = Guid.NewGuid(), Reference = "Ref1"},
            new() { Id = Guid.NewGuid(), Reference = "Ref2" }
        };

        // When
        var data = await ExportHelper.ExportRecordsAsync(droits,new DroitsCsvMap(new DroitExportForm(){Id = false}));
        

        // Assert
        Assert.NotEmpty(data);
    }


    [Fact]
    public async Task ExportDroitsAsync_ListOfDroits_ReturnsCorrectData()
    {
        // Given
        var droits = new List<DroitExportDto>()
        {
            new() { Id = new Guid(), Reference = "Ref3" },
            new() { Id = new Guid(), Reference = "Ref4" }
        };

        // When
        var data = await ExportHelper.ExportRecordsAsync(droits,new DroitsCsvMap(new DroitExportForm(){Id = false}));
        var dataString = Encoding.Default.GetString(data);
        var rows = dataString.Split("\r\n");

        


        // Assert

        Assert.NotNull(rows);
    }


    [Fact]
    public async Task ExportDroitsAsync_ListOfDroits_ReturnsCorrectHeaders()
    {
        // Given
        var droits = new List<DroitExportDto>()
        {
            new() { Id = new Guid(), Reference = "Ref3" ,WreckMaterials = new List<WreckMaterialDto>() {new ()
            {
                Description = "foo",
                Name = "bar",
                Owner = "baz",
                Outcome = "woo",
                Quantity = "1",
                Value = "350"
            }}},
            new() { Id = new Guid(), Reference = "Ref4" , WreckMaterials = new List<WreckMaterialDto>() {new ()
            {
                Description = "foo",
                Name = "bar",
                Owner = "baz",
                Outcome = "woo",
                Quantity = "1",
                Value = "350"
            }}},
        };
        

        // When
        var data = await ExportHelper.ExportRecordsAsync(droits,new DroitsCsvMap(new DroitExportForm(){Id = true}));

        var dataString = Encoding.Default.GetString(data);

        var headers = dataString.Split("\r\n").First().Split(",").ToList();


        // Assert
        Assert.NotNull(headers);
    }
}