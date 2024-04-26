using AutoMapper;
using Droits.Data.Mappers.Imports;
using Droits.Models.DTOs.Imports;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Helpers.Extensions;
using Newtonsoft.Json;

namespace Droits.Tests.UnitTests.Data.Mappers.Import;

public class AccessDroitMappingProfileUnitTests
{
    private readonly IMapper _mapper;

    public AccessDroitMappingProfileUnitTests()
    {
        var profile = new AccessDroitMappingProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
        _mapper = new Mapper(configuration);
    }

    [Fact]
    public void TestMapping_AccessDtoToDroit()
    {
        // Arrange
        var dto = new AccessDto()
        {
            DroitNumber = "TestRef123",
            DateReported = "2022-04-20",
            DateFound = "2022-04-21",
            UkWaters = "true",
            SalvageAwardClaimed = "false",
            NatureOfServices = "Salvage",
            Duration = "2",
            EstimatedCostOfServices = "1000.50",
            WreckName = "TestWreck",
            YearOfLoss = "2020",
            WreckConstructionDetails = "Wooden",
            Agent = "TestAgent",
            District = "TestDistrict",
            ClosureOfDroits = "2022-04-22",
            Remarks = "TestRemarks",
            FileRef = "TestFileRef",
            RecoveredFrom = "TestRecoveredFrom"
        };

        // Act
        var droit = _mapper.Map<Droit>(dto);

        // Assert
        Assert.Equal(dto.DroitNumber, droit.Reference);
        Assert.True(droit.ImportedFromLegacy);
        Assert.Equal(JsonConvert.SerializeObject(dto), droit.OriginalSubmission);
        Assert.Equal(dto.DateReported.AsDateTime(), droit.ReportedDate);
        Assert.Equal(dto.DateFound.AsDateTime(), droit.DateFound);
        Assert.Equal(dto.GetLocationDescription(), droit.LocationDescription);
        Assert.True(droit.InUkWaters);
        Assert.False(droit.SalvageAwardClaimed);
        Assert.Equal(dto.NatureOfServices, droit.ServicesDescription);
        Assert.Equal(dto.Duration, droit.ServicesDuration);
        Assert.Equal(double.Parse(dto.EstimatedCostOfServices), droit.ServicesEstimatedCost);
        Assert.Equal(dto.WreckName, droit.ReportedWreckName);
        Assert.Equal(int.Parse(dto.YearOfLoss), droit.ReportedWreckYearSunk);
        Assert.Equal(dto.WreckConstructionDetails, droit.ReportedWreckConstructionDetails);
        Assert.Equal(dto.Agent, droit.Agent);
        Assert.Equal(dto.District, droit.District);
        Assert.Equal(dto.ClosureOfDroits.AsDateTime(), droit.ClosedDate);
        Assert.Equal(dto.GetDepth(), droit.Depth);
        Assert.Equal(DroitStatus.Closed, droit.Status); // Assuming Closed if ClosureOfDroits is not null
        Assert.Equal(dto.Remarks, droit.LegacyRemarks);
        Assert.Equal(dto.FileRef, droit.LegacyFileReference);
        Assert.Equal(dto.RecoveredFrom,droit.RecoveredFromLegacy);
        Assert.Equal(dto.RecoveredFrom.AsRecoveredFromEnum(), droit.RecoveredFrom);
        Assert.Empty(droit.WreckMaterials);
    }
        
    [Fact]
    public void TestMapping_AccessDtoToDroit_NullValues()
    {
        // Arrange
        var dto = new AccessDto()
        {
            DroitNumber = null,
            DateReported = null,
            DateFound = null,
            UkWaters = null,
            SalvageAwardClaimed = null,
            NatureOfServices = null,
            Duration = null,
            EstimatedCostOfServices = null,
            WreckName = null,
            YearOfLoss = null,
            WreckConstructionDetails = null,
            Agent = null,
            District = null,
            ClosureOfDroits = null,
            Remarks = null,
            FileRef = null,
            RecoveredFrom = null
        };

        // Act
        var droit = _mapper.Map<Droit>(dto);

        // Assert
        Assert.Null(droit.Reference);
        Assert.True(droit.ImportedFromLegacy);
        Assert.Equal(JsonConvert.SerializeObject(dto), droit.OriginalSubmission);
        Assert.Equal(DateTime.MinValue.Date, droit.ReportedDate);
        Assert.Equal(DateTime.MinValue.Date, droit.DateFound);
        Assert.Equal(string.Empty,droit.LocationDescription);
        Assert.False(droit.InUkWaters);
        Assert.False(droit.SalvageAwardClaimed);
        Assert.Null(droit.ServicesDescription);
        Assert.Null(droit.ServicesDuration);
        Assert.Null(droit.ServicesEstimatedCost);
        Assert.Null(droit.ReportedWreckName);
        Assert.Null(droit.ReportedWreckYearSunk);
        Assert.Null(droit.ReportedWreckConstructionDetails);
        Assert.Null(droit.Agent);
        Assert.Null( droit.District);
        Assert.Equal(DateTime.MinValue.Date, droit.ClosedDate);
        Assert.Null(droit.Depth);
        Assert.Equal(DroitStatus.Received, droit.Status);
        Assert.Null(droit.LegacyRemarks);
        Assert.Null(droit.LegacyFileReference);
        Assert.Null(droit.RecoveredFromLegacy);
        Assert.Null(droit.RecoveredFrom);
        Assert.Empty(droit.WreckMaterials);
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