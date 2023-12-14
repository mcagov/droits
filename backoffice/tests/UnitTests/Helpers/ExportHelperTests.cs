using System.Text;
using Droits.Helpers;
using Droits.Models.DTOs;
using Droits.Models.DTOs.Exports;

namespace Droits.Tests.UnitTests.Helpers;

public class ExportHelperTests
{
    
    private readonly List<string> _expectedDroitHeaders = new()
        {
            "Id", "Reference", "Created", "LastModified", "WreckName", "SalvorName", "AssignedTo", "Status", "TriageNumber"
        };
    
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
        var data = await ExportHelper.ExportRecordsAsync(droits);

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
        var data = await ExportHelper.ExportRecordsAsync(droits);
        var dataString = Encoding.Default.GetString(data);
        var rows = dataString.Split("\r\n");

        


        // Assert
        var referenceIndex = _expectedDroitHeaders.IndexOf("Reference");

        var index = 0;
        foreach ( var row in rows.Skip(1).Where(r => !string.IsNullOrWhiteSpace(r)) )
        {
            var cols = row.Split(",").ToList();

            Assert.True(cols.Count > referenceIndex);
            var reference = cols[referenceIndex];
            Assert.Equal(droits[index].Reference, reference);

            index++;
        }
    }


    [Fact]
    public async Task ExportDroitsAsync_ListOfDroits_ReturnsCorrectHeaders()
    {
        // Given
        var droits = new List<DroitExportDto>()
        {
            new() { Id = new Guid(), Reference = "Ref3" },
            new() { Id = new Guid(), Reference = "Ref4" }
        };
        

        // When
        var data = await ExportHelper.ExportRecordsAsync(droits);
        var dataString = Encoding.Default.GetString(data);

        var headers = dataString.Split("\r\n").First().Split(",").ToList();


        // Assert
        Assert.Equal(_expectedDroitHeaders, headers);
    }
}