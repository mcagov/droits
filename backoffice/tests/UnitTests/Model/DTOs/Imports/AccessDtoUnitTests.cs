using AutoMapper;
using Droits.Data.Mappers;
using Droits.Data.Mappers.Imports;
using Droits.Models.DTOs.Imports;
using Droits.Models.Entities;
using Droits.Models.Enums;

namespace Droits.Tests.UnitTests.Model.DTOs.Imports;

public class AccessDtoUnitTests
{
    private readonly IMapper _mapper;
    public AccessDtoUnitTests()
    {
        var profile = new AccessDroitMappingProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
        _mapper = new Mapper(configuration);
    }

    [Theory]
    [InlineData("18m MHWS", null)]
    [InlineData("5,800m", 5800)]
    [InlineData("3200m", 3200)]
    [InlineData("Max 30m", 30)]
    [InlineData("190ft", null)]
    [InlineData("1m LW", 1)]
    [InlineData("27.2", 27)]
    [InlineData("<8m", 8)]
    [InlineData("5m above datum", 5)]
    [InlineData("Above low tide", null)]
    [InlineData("38m max low water", 38)]
    [InlineData("48-53", 50)]
    [InlineData("30 fathoms", null)]
    [InlineData("3.5 inches", null)]
    [InlineData("<10m", 10)]
    [InlineData("28.1m high neaps", 28)]
    [InlineData("~25m", 25)]
    [InlineData("16 & 13m", null)]
    [InlineData("-10m", 10)]
    [InlineData(">10m", 10)]
    [InlineData("Foreshore", null)]
    [InlineData("5m (at high water)", 5)]
    [InlineData("28m to 30m", 29)]
    [InlineData("Approx 30m", 30)]
    [InlineData("-2 BCD", null)]
    [InlineData("Unknown", null)]
    [InlineData("n/a", null)]
    [InlineData("c. 5m", 5)]
    [InlineData("-22m", 22)]
    [InlineData("20 MSW", null)]
    [InlineData("20MSW", null)]
    [InlineData("object found on wreck next to winch", null)]
    [InlineData("02.22.318 brick 2 to 6: 50.35.300", null)]
    [InlineData(null, null)]
    [InlineData("40-50m", 45)]
    [InlineData("52.4", 52)]
    [InlineData("53m", 53)]
    [InlineData("36m", 36)]
    [InlineData("40m-50m", 45)]
    [InlineData("4 0 -50m", 45)]
    [InlineData("test", null)]
    [InlineData("40metres-unknown", null)]
    [InlineData("quite deep", null)]
    public void TestGetDepth(string? depthInput, int? expected)
    {
        var dto = new AccessDto()
        {
            Depth = depthInput
        };
        
        // Act
        var result = dto.GetDepth();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TestMappingHappyDto_ToDroit()
    {
        var dto = new AccessDto()
        {
            DroitNumber = "123/456",
            District = "foo",
            UkWaters = "true",
            WreckAge = "modern",
            WarWreck = "false",
            DateFound = "11/11/11",
            DateReported = "11/11/11",
            Depth = "20",
            LatitudeA = "foo",
            LongitudeA = "bar",
            NatureOfServices = "salvage",
            EstimatedCostOfServices = "1",
            WreckName = "foo",
            YearOfLoss = "2000",
            WreckConstructionDetails = "bar",
            Agent = "bar",
            FileRef = "123",
            RecoveredFrom = "afloat",
            SalvageAwardClaimed = "false",
            SalvageNo = "false",
            Remarks = "foo",
            ClosureOfDroits = "12/11/11",
        };

        var droit = _mapper.Map<Droit>(dto);
        
        Assert.Equal("123/456",droit.Reference);
        Assert.Equal("foo",droit.District);
        Assert.True(droit.InUkWaters);
        Assert.True(droit.ImportedFromLegacy);
        Assert.Equal(new DateTime(2011,11,11),droit.DateFound);
        Assert.Equal(new DateTime(2011,11,11),droit.ReportedDate);
        Assert.Equal(new DateTime(2011,11,12),droit.ClosedDate);
        Assert.Equal(20,droit.Depth);
        Assert.Equal(RecoveredFrom.Afloat,droit.RecoveredFrom);
        Assert.Equal(DroitStatus.Closed,droit.Status);
        Assert.Equal("foo",droit.LegacyRemarks);
        Assert.Contains("foo",droit.LocationDescription);
        Assert.Contains("bar",droit.LocationDescription);
        Assert.False(droit.SalvageAwardClaimed);
        Assert.Equal("salvage",droit.ServicesDescription);
        Assert.Equal(1,droit.ServicesEstimatedCost);
        Assert.Equal("foo",droit.ReportedWreckName);
        Assert.Equal(2000,droit.ReportedWreckYearSunk);
        Assert.Equal("bar",droit.ReportedWreckConstructionDetails);
        Assert.Equal("bar",droit.Agent);
        Assert.Equal("123",droit.LegacyFileReference);

    }
    
    [Fact]
    public void TestMappingDifficultDtoTo_Droit()
    {
        var dto = new AccessDto()
        {
            DroitNumber = "123/456",
            District = "foo",
            UkWaters = "true",
            WreckAge = "modern",
            WarWreck = "false",
            DateFound = "11/11/11",
            DateReported = "not a date",
            Depth = "20.5m-21.500 meters",
            LatitudeA = "foo",
            LongitudeA = "bar",
            NatureOfServices = "salvage",
            EstimatedCostOfServices = "100 pounds sterling",
            WreckName = "foo",
            YearOfLoss = "two thousand and one",
            WreckConstructionDetails = "bar",
            Agent = "bar",
            FileRef = "123",
            RecoveredFrom = "somewhere",
            SalvageAwardClaimed = "false",
            SalvageNo = "false",
            Remarks = "foo",
            ClosureOfDroits = "12/11/11",
        };

        var droit = _mapper.Map<Droit>(dto);
        
        Assert.Equal("123/456",droit.Reference);
        Assert.Equal("foo",droit.District);
        Assert.True(droit.InUkWaters);
        Assert.True(droit.ImportedFromLegacy);
        Assert.Equal(new DateTime(2011,11,11),droit.DateFound);
        Assert.Equal(new DateTime(1,1,1),droit.ReportedDate);
        Assert.Equal(new DateTime(2011,11,12),droit.ClosedDate);
        Assert.Equal(21,droit.Depth);
        Assert.Null(droit.RecoveredFrom);
        Assert.Equal(DroitStatus.Closed,droit.Status);
        Assert.Equal("foo",droit.LegacyRemarks);
        Assert.Contains("foo",droit.LocationDescription);
        Assert.Contains("bar",droit.LocationDescription);
        Assert.False(droit.SalvageAwardClaimed);
        Assert.Equal("salvage",droit.ServicesDescription);
        Assert.Null(droit.ServicesEstimatedCost);
        Assert.Equal("foo",droit.ReportedWreckName);
        Assert.Null(droit.ReportedWreckYearSunk);
        Assert.Equal("bar",droit.ReportedWreckConstructionDetails);
        Assert.Equal("bar",droit.Agent);
        Assert.Equal("123",droit.LegacyFileReference);

    }

}