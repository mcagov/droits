using Droits.Helpers.SearchHelpers;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Models.FormModels.SearchFormModels;

namespace Droits.Tests.UnitTests.Helpers.SearchHelpers;

public class DroitQueryBuilderUnitTests
{
    [Fact]
    public void BuildQuery_WithValidName_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new DroitSearchForm { Reference = "Ref" };
        var droits = new List<Droit>
        {
            new() { Id = Guid.NewGuid(), Reference = "Ref" },
            new() { Id = Guid.NewGuid(), Reference = "AnotherRef" },
            new() { Id = Guid.NewGuid(), Reference = "NotMatching" }
        }.AsQueryable();

        // Act
        var result = DroitQueryBuilder.BuildQuery(form, droits, false);

        // Assert
        Assert.Equal(2, result.Count()); 
        Assert.True(result.All(d => d.Reference.ToLower().Contains("ref")));
    }
    
    public static TheoryData<DateTime, DateTime> DateTimeTestData =>
        new()
        {
            { new DateTime(2023, 10, 01), new DateTime(2023, 10, 10) },
        };

    [Theory]
    [MemberData(nameof(DateTimeTestData))]
    public void BuildQuery_WithDroitDateSearchFields_ReturnsFilteredQuery(DateTime dateFrom, DateTime dateTo)
    {
        // Arrange
        var form = new DroitSearchForm
        {
            CreatedFrom = dateFrom,
            CreatedTo = dateTo,
            LastModifiedFrom = dateFrom,
            LastModifiedTo = dateTo,
            ReportedDateFrom = dateFrom,
            ReportedDateTo = dateTo,
            DateFoundFrom = dateFrom,
            DateFoundTo = dateTo,
        };
        var droits = new List<Droit>
        {
            new()
            {
                Id = Guid.NewGuid(), Reference = "Ref",
                Created = new DateTime(2023,10,10),
                LastModified = new DateTime(2023,10,10),
                ReportedDate = new DateTime(2023,10,10),
                DateFound = new DateTime(2023,10,10)
            },
            new()
            {
                Id = Guid.NewGuid(), Reference = "AnotherRef" ,
                Created = new DateTime(2023,10,01),
                LastModified = new DateTime(2023,10,01),
                ReportedDate = new DateTime(2023,10,01),
                DateFound = new DateTime(2023,10,01)
            },
            new()
            {
                Id = Guid.NewGuid(), Reference = "NotMatchingRef" ,
                Created = new DateTime(2023,11,01),
                LastModified = new DateTime(2023,11,01),
                ReportedDate = new DateTime(2023,11,01),
                DateFound = new DateTime(2023,11,01)
            },
        }.AsQueryable();
    
        // Act
        var result = DroitQueryBuilder.BuildQuery(form, droits, false);
    
        // Assert
        Assert.Equal(2, result.Count());
        Assert.DoesNotContain(result.ToList(), d => d.Reference == "NotMatchingRef");
    }
    
    [Theory]
    [MemberData(nameof(DateTimeTestData))]
    public void BuildQuery_WithDroitDateSearchFields_ReturnsFilteredQueryWithInclusiveDates(DateTime dateFrom, DateTime dateTo)
    {
        // Arrange
        var form = new DroitSearchForm
        {
            CreatedFrom = dateFrom,
            LastModifiedTo = dateTo,
            ReportedDateFrom = dateFrom,
            DateFoundTo = dateTo,
        };
        var droits = new List<Droit>
        {
            new()
            {
                Id = Guid.NewGuid(), Reference = "Ref",
                Created = new DateTime(2023,10,10),
                LastModified = new DateTime(2023,10,10),
                ReportedDate = new DateTime(2023,10,10),
                DateFound = new DateTime(2023,10,10)
            },
            new()
            {
                Id = Guid.NewGuid(), Reference = "AnotherRef" ,
                Created = new DateTime(2023,10,01),
                LastModified = new DateTime(2023,10,01),
                ReportedDate = new DateTime(2023,10,01),
                DateFound = new DateTime(2023,10,01)
            },
            new()
            {
                Id = Guid.NewGuid(), Reference = "NotMatchingRef" ,
                Created = new DateTime(2023,9,01),
                LastModified = new DateTime(2023,11,01),
                ReportedDate = new DateTime(2023,9,01),
                DateFound = new DateTime(2023,11,01)
            },
        }.AsQueryable();
    
        // Act
        var result = DroitQueryBuilder.BuildQuery(form, droits, false);
    
        // Assert
        Assert.Equal(2, result.Count());
        Assert.DoesNotContain(result.ToList(), d => d.Reference == "NotMatchingRef");
        Assert.Contains(result.ToList(), d => d.Reference == "Ref");
        Assert.Contains(result.ToList(), d => d.Reference == "AnotherRef");

    }
    
    [Fact]
    public void BuildQuery_WithDroitBooleanAndGuidSearchFields_ReturnsFilteredQuery()
    {
        // Arrange
        var assignedUserId = Guid.NewGuid();
        var form = new DroitSearchForm
        {
            IsHazardousFind = true , IsDredge = false,
            IsIsolatedFind = true, AssignedToUserId = assignedUserId
        };
        var droits = new List<Droit>
        {
            new()
            {
                Id = Guid.NewGuid(), Reference = "MatchingDroit",
                IsHazardousFind = true , IsDredge = false,
                WreckId = null, AssignedToUserId = assignedUserId
            },
            new() {
                Id = Guid.NewGuid(), Reference = "PartiallyMatchingDroit",
                IsHazardousFind = true , IsDredge = true,
                WreckId = null, AssignedToUserId = assignedUserId 
            },
            new() {
                Id = Guid.NewGuid(), Reference = "NonMatchingDroit",
                IsHazardousFind = false , IsDredge = true,
                WreckId = null, AssignedToUserId = Guid.NewGuid() 
            },
        }.AsQueryable();
    
        // Act
        var result = DroitQueryBuilder.BuildQuery(form, droits, false);
    
        // Assert
        Assert.Equal(1, result.Count()); 
        Assert.Equal("MatchingDroit",result.First().Reference);
    }
    
    [Fact]
    public void BuildQuery_WithASingleDroitStatusSearchFields_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new DroitSearchForm
        {
            StatusList = new List<DroitStatus>() { DroitStatus.Received }
        };
        var droits = new List<Droit>
        {
            new()
            {
                Id = Guid.NewGuid(), Reference = "MatchingDroit",
                Status = DroitStatus.Received
            },
            new() {
                Id = Guid.NewGuid(), Reference = "NonMatchingDroit",
                Status = DroitStatus.Closed
            },
        }.AsQueryable();
    
        // Act
        var result = DroitQueryBuilder.BuildQuery(form, droits, false);
    
        // Assert
        Assert.Equal(1, result.Count()); 
        Assert.Equal("MatchingDroit",result.First().Reference);
    }
    
    [Fact]
    public void BuildQuery_WithMultipleDroitStatusSearchFields_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new DroitSearchForm
        {
            StatusList = new List<DroitStatus>() { DroitStatus.Received , DroitStatus.Closed}
        };
        var droits = new List<Droit>
        {
            new()
            {
                Id = Guid.NewGuid(), Reference = "MatchingDroit",
                Status = DroitStatus.Received
            },
            new() {
                Id = Guid.NewGuid(), Reference = "AnotherMatchingDroit",
                Status = DroitStatus.Closed
            },
            new() {
                Id = Guid.NewGuid(), Reference = "NotMatchingDroit",
                Status = DroitStatus.QcApproved
            },
        }.AsQueryable();
    
        // Act
        var result = DroitQueryBuilder.BuildQuery(form, droits, false);
    
        // Assert
        Assert.Equal(2, result.Count()); 
        Assert.True(result.Any(d => d.Reference == "MatchingDroit"));
    }
    
    [Fact]
    public void BuildQuery_WithWreckSearchFields_ReturnsFilteredQuery()
    {
        // Arrange
        var matchingWreck = new Wreck() { Id = Guid.NewGuid(), Name = "Test"};
        var partiallyMatchingWreck = new Wreck() { Id = Guid.NewGuid(), Name = "AnotherTest"};
        var form = new DroitSearchForm { WreckName = "Test" };
        var droits = new List<Droit>
        {
            new()
            {
                Id = Guid.NewGuid(), Reference = "MatchingDroit",
                Wreck = matchingWreck,
                WreckId = matchingWreck.Id
            },
            new() {
                Id = Guid.NewGuid(), Reference = "PartiallyMatchingDroit",
                Wreck = partiallyMatchingWreck,
                WreckId = partiallyMatchingWreck.Id
            },
            new() {
                Id = Guid.NewGuid(), Reference = "NotMatchingDroit",
                Wreck = new Wreck(),
                WreckId = Guid.NewGuid()
            },
        }.AsQueryable();
    
        // Act
        var result = DroitQueryBuilder.BuildQuery(form, droits, false);
    
        // Assert
        Assert.Equal(2, result.Count()); 
        Assert.True(result.Any(d => d.Reference == "MatchingDroit"));
        Assert.True(result.Any(d => d.Reference == "PartiallyMatchingDroit"));
    }
    
    [Fact]
    public void BuildQuery_WithWreckOwnerSearchField_ReturnsFilteredQuery()
    {
        // Arrange
        var matchingWreck = new Wreck() { Id = Guid.NewGuid(), OwnerName = "Test"};
        var partiallyMatchingWreck = new Wreck() { Id = Guid.NewGuid(), OwnerName = "AnotherTest"};
        var form = new DroitSearchForm { OwnerName = "Test" };
        var droits = new List<Droit>
        {
            new()
            {
                Id = Guid.NewGuid(), Reference = "MatchingDroit",
                Wreck = matchingWreck,
                WreckId = matchingWreck.Id
            },
            new() {
                Id = Guid.NewGuid(), Reference = "PartiallyMatchingDroit",
                Wreck = partiallyMatchingWreck,
                WreckId = partiallyMatchingWreck.Id
            },
            new() {
                Id = Guid.NewGuid(), Reference = "NotMatchingDroit",
                Wreck = new Wreck(),
                WreckId = Guid.NewGuid()
            },
        }.AsQueryable();
    
        // Act
        var result = DroitQueryBuilder.BuildQuery(form, droits, false);
    
        // Assert
        Assert.Equal(2, result.Count()); 
        Assert.True(result.Any(d => d.Reference == "MatchingDroit"));
        Assert.True(result.Any(d => d.Reference == "PartiallyMatchingDroit"));
    }
    
    [Fact]
    public void BuildQuery_WithSalvorSearchFields_ReturnsFilteredQuery()
    {
        // Arrange
        var matchingSalvor = new Salvor() { Id = Guid.NewGuid(), Name = "Test"};
        var partiallyMatchingSalvor = new Salvor() { Id = Guid.NewGuid(), Name = "AnotherTest"};
        var form = new DroitSearchForm { SalvorName = "Test" };
        var droits = new List<Droit>
        {
            new()
            {
                Id = Guid.NewGuid(), Reference = "MatchingDroit",
                Salvor = matchingSalvor,
                SalvorId = matchingSalvor.Id
            },
            new() {
                Id = Guid.NewGuid(), Reference = "PartiallyMatchingDroit",
                Salvor = partiallyMatchingSalvor,
                SalvorId = partiallyMatchingSalvor.Id
            },
            new() {
                Id = Guid.NewGuid(), Reference = "NotMatchingDroit",
                Salvor = new Salvor(),
                SalvorId = Guid.NewGuid()
            },
        }.AsQueryable();
    
        // Act
        var result = DroitQueryBuilder.BuildQuery(form, droits, false);
    
        // Assert
        Assert.Equal(2, result.Count()); 
        Assert.True(result.Any(d => d.Reference == "MatchingDroit"));
        Assert.True(result.Any(d => d.Reference == "PartiallyMatchingDroit"));
    }
    
    [Fact]
    public void BuildQuery_WithLocationSearchFields_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new DroitSearchForm { LatitudeFrom = 40,LatitudeTo = 60,
            LongitudeFrom = 40,LongitudeTo = 60,DepthFrom = 0,DepthTo = 100,
            LocationDescription = "Test"};
        var droits = new List<Droit>
        {
            new()
            {
                Id = Guid.NewGuid(), Reference = "MatchingDroit",
                Latitude = 50, Longitude = 50,
                Depth = 50, LocationDescription = "Test"
            },
            new() {
                Id = Guid.NewGuid(), Reference = "AnotherMatchingDroit",
                Latitude = 40, Longitude = 60,
                Depth = 100, LocationDescription = "AnotherTest"
            },
            new() {
                Id = Guid.NewGuid(), Reference = "OnlyPartiallyMatchingDroit",
                Latitude = 20, Longitude = 50,
                Depth = 50, LocationDescription = "OnlyPartiallyMatching"
            },
            new() {
                Id = Guid.NewGuid(), Reference = "NotMatchingDroit",
                Latitude = 0, Longitude = 100,
                Depth = 200, LocationDescription = "NotMatching"
            },
        }.AsQueryable();
    
        // Act
        var result = DroitQueryBuilder.BuildQuery(form, droits, false);
    
        // Assert
        Assert.Equal(2, result.Count()); 
        Assert.True(result.Any(d => d.Reference == "MatchingDroit"));
        Assert.True(result.Any(d => d.Reference == "AnotherMatchingDroit"));
        Assert.False(result.Any(d => d.Reference == "OnlyPartiallyMatchingDroit"));
    }
    
     [Fact]
    public void BuildQuery_WithLocationSearchFieldsWhereSomeFieldsAreNull_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new DroitSearchForm { LatitudeFrom = 40, LongitudeTo = 60, DepthFrom = 100,
            LocationDescription = "Test"};
        var droits = new List<Droit>
        {
            new()
            {
                Id = Guid.NewGuid(), Reference = "MatchingDroit",
                Latitude = 50, Longitude = 50,
                Depth = 150, LocationDescription = "Test"
            },
            new() {
                Id = Guid.NewGuid(), Reference = "AnotherMatchingDroit",
                Latitude = 40, Longitude = 60,
                Depth = 100, LocationDescription = "AnotherTest"
            },
            new() {
                Id = Guid.NewGuid(), Reference = "OnlyPartiallyMatchingDroit",
                Latitude = 20, Longitude = 50,
                Depth = 50, LocationDescription = "OnlyPartiallyMatching"
            },
            new() {
                Id = Guid.NewGuid(), Reference = "NotMatchingDroit",
                Latitude = 0, Longitude = 100,
                Depth = 0, LocationDescription = "NotMatching"
            },
            new() {
                Id = Guid.NewGuid(), Reference = "OnlyMatchingDescriptionDroit",
                Latitude = 0, Longitude = 100,
                Depth = 0, LocationDescription = "Test"
            },
        }.AsQueryable();
    
        // Act
        var result = DroitQueryBuilder.BuildQuery(form, droits, false);
    
        // Assert
        Assert.Equal(2, result.Count()); 
        Assert.True(result.Any(d => d.Reference == "MatchingDroit"));
        Assert.True(result.Any(d => d.Reference == "AnotherMatchingDroit"));
        Assert.False(result.Any(d => d.Reference == "OnlyPartiallyMatchingDroit"));
    }
    
    [Fact]
    public void BuildQuery_WithOtherLocationSearchFields_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new DroitSearchForm { InUkWaters = true, RecoveredFromList = new List<RecoveredFrom>()
            {
                RecoveredFrom.Seabed
            }};
        var droits = new List<Droit>
        {
            new()
            {
                Id = Guid.NewGuid(), Reference = "MatchingDroit",
                InUkWaters = true, RecoveredFrom = RecoveredFrom.Seabed
            },
            new() {
                Id = Guid.NewGuid(), Reference = "NotMatching",
                InUkWaters = true, RecoveredFrom = RecoveredFrom.Afloat
            },
        }.AsQueryable();
    
        // Act
        var result = DroitQueryBuilder.BuildQuery(form, droits, false);
    
        // Assert
        Assert.Equal(1, result.Count()); 
        Assert.True(result.Any(d => d.Reference == "MatchingDroit"));
    }
    
    [Fact]
    public void BuildQuery_WithWreckMaterialSearchFields_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new DroitSearchForm
        {
            WreckMaterial = "Test", WreckMaterialOwner = "Owner",
            ValueConfirmed = true
        };
        var matchingWreckMaterial = new WreckMaterial() { Description = "Test", WreckMaterialOwner = "Owner", 
            ValueConfirmed = true};
        var notMatchingWreckMaterial = new WreckMaterial() { Description = "Not Matching", WreckMaterialOwner = "None", 
            ValueConfirmed = false};
        var anotherMatchingWreckMaterial = new WreckMaterial() { Description = "Test with extra", WreckMaterialOwner = "Same Owner", 
            ValueConfirmed = true};
        var droits = new List<Droit>
        {
            new()
            {
                Id = Guid.NewGuid(), Reference = "MatchingDroit",
                WreckMaterials = new List<WreckMaterial>() {matchingWreckMaterial}
            },
            new()
            {
                Id = Guid.NewGuid(), Reference = "AnotherMatchingDroit",
                WreckMaterials = new List<WreckMaterial>() {anotherMatchingWreckMaterial}
            },
            new()
            {
                Id = Guid.NewGuid(), Reference = "NotMatchingDroit",
                WreckMaterials = new List<WreckMaterial>() {notMatchingWreckMaterial}
            },
        }.AsQueryable();
    
        // Act
        var result = DroitQueryBuilder.BuildQuery(form, droits, false);
    
        // Assert
        Assert.Equal(2, result.Count()); 
        Assert.True(result.Any(d => d.Reference == "MatchingDroit"));
        Assert.True(result.Any(d => d.Reference == "AnotherMatchingDroit"));
        Assert.False(result.Any(d => d.Reference == "NotMatchingDroit"));
    }
    
    [Fact]
    public void BuildQuery_WithWreckMaterialIntSearchFields_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new DroitSearchForm
        {
            QuantityFrom = 1,QuantityTo = 3,
            // ValueFrom = 1,ValueTo = 3,
            // ReceiverValuationFrom = 1,ReceiverValuationTo = 3
        };
        var matchingWreckMaterial = new WreckMaterial() { Quantity = 2, Value = 2, ReceiverValuation = 2};
        var notMatchingWreckMaterial = new WreckMaterial() { Quantity = 10, Value = 10, ReceiverValuation = 10};
        var anotherMatchingWreckMaterial = new WreckMaterial() { Quantity = 2, Value = 1, ReceiverValuation = 3};
        var droits = new List<Droit>
        {
            new()
            {
                Id = Guid.NewGuid(), Reference = "MatchingDroit",
                WreckMaterials = new List<WreckMaterial>() { matchingWreckMaterial }
            },
            new()
            {
                Id = Guid.NewGuid(), Reference = "AnotherMatchingDroit",
                WreckMaterials = new List<WreckMaterial>() { anotherMatchingWreckMaterial }
            },
            new()
            {
                Id = Guid.NewGuid(), Reference = "NotMatchingDroit",
                WreckMaterials = new List<WreckMaterial>() { notMatchingWreckMaterial }
            },
        }.AsQueryable();
    
        // Act
        var result = DroitQueryBuilder.BuildQuery(form, droits, false);
    
        // Assert
        Assert.Equal(2, result.Count()); 
        Assert.True(result.Any(d => d.Reference == "MatchingDroit"));
        Assert.True(result.Any(d => d.Reference == "AnotherMatchingDroit"));
        Assert.False(result.Any(d => d.Reference == "NotMatchingDroit"));
    }
    
    [Fact]
    public void BuildQuery_WithWreckMaterialIntSearchFieldsWhereSomeAreNull_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new DroitSearchForm
        {
            QuantityFrom = 2, ValueTo = 1000,
            ReceiverValuationFrom = 200
        };
        var matchingWreckMaterial = new WreckMaterial() { Quantity = 4, Value = 300, ReceiverValuation = 325};
        var notMatchingWreckMaterial = new WreckMaterial() { Quantity = 10, Value = 300, ReceiverValuation = 100};
        var anotherMatchingWreckMaterial = new WreckMaterial() { Quantity = 2, Value = 100, ReceiverValuation = 325};
        var droits = new List<Droit>
        {
            new()
            {
                Id = Guid.NewGuid(), Reference = "MatchingDroit",
                WreckMaterials = new List<WreckMaterial>() {matchingWreckMaterial}
            },
            new()
            {
                Id = Guid.NewGuid(), Reference = "AnotherMatchingDroit",
                WreckMaterials = new List<WreckMaterial>() {anotherMatchingWreckMaterial}
            },
            new()
            {
                Id = Guid.NewGuid(), Reference = "NotMatchingDroit",
                WreckMaterials = new List<WreckMaterial>() {notMatchingWreckMaterial}
            },
        }.AsQueryable();
    
        // Act
        var result = DroitQueryBuilder.BuildQuery(form, droits, false);
    
        // Assert
        Assert.Equal(2, result.Count()); 
        Assert.True(result.Any(d => d.Reference == "MatchingDroit"));
        Assert.True(result.Any(d => d.Reference == "AnotherMatchingDroit"));
        Assert.False(result.Any(d => d.Reference == "NotMatchingDroit"));
    }
    
    [Fact]
    public void BuildQuery_WithWreckMaterialQuantitySearchField_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new DroitSearchForm
        {
            QuantityFrom = 2, QuantityTo = 4
        };
        var matchingWreckMaterial = new WreckMaterial() { Quantity = 2 };
        var notMatchingWreckMaterial = new WreckMaterial() { Quantity = 5 };
        var anotherNotMatchingWreckMaterial = new WreckMaterial() { Quantity = 1 };

        var droits = new List<Droit>
        {
            new()
            {
                Id = Guid.NewGuid(), Reference = "MatchingDroit",
                WreckMaterials = new List<WreckMaterial>() {matchingWreckMaterial}
            },
            new()
            {
                Id = Guid.NewGuid(), Reference = "NotMatchingDroit",
                WreckMaterials = new List<WreckMaterial>() {notMatchingWreckMaterial,anotherNotMatchingWreckMaterial}
            },
        }.AsQueryable();
    
        // Act
        var result = DroitQueryBuilder.BuildQuery(form, droits, false);
    
        // Assert
        Assert.Equal(1, result.Count()); 
        Assert.True(result.Any(d => d.Reference == "MatchingDroit"));
        Assert.False(result.Any(d => d.Reference == "NotMatchingDroit"));
    }
    
    [Fact]
    public void BuildQuery_WithSalvageSearchFields_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new DroitSearchForm
        {
            ServicesDescription = "Test", ServicesDuration = "Time",
            ServicesEstimatedCostFrom = 2, ServicesEstimatedCostTo = 10,
            SalvageClaimAwardedFrom = 2, SalvageClaimAwardedTo = 5
        };
        var salvor = new Salvor() {};
        var droits = new List<Droit>
        {
            new()
            {
                Id = Guid.NewGuid(), Reference = "MatchingDroit",
                ServicesDescription = "Test",ServicesDuration = "Time",
                ServicesEstimatedCost = 10, SalvageClaimAwarded = 4,
                Salvor = salvor
            },
            new()
            {
                Id = Guid.NewGuid(), Reference = "AnotherMatchingDroit",
                ServicesDescription = "AnotherTest", ServicesDuration = "AnotherTime",
                ServicesEstimatedCost = 10,SalvageClaimAwarded = 5,
                Salvor = salvor
            },
            new()
            {
                Id = Guid.NewGuid(), Reference = "NotMatchingDroit",
                ServicesDescription = "NotMatching", ServicesDuration = "",
                ServicesEstimatedCost = 11,SalvageClaimAwarded = 7,
                Salvor = salvor
            }
        }.AsQueryable();
    
        // Act
        var result = DroitQueryBuilder.BuildQuery(form, droits, false);
    
        // Assert
        Assert.Equal(2, result.Count()); 
        Assert.True(result.Any(d => d.Reference == "MatchingDroit"));
        Assert.True(result.Any(d => d.Reference == "AnotherMatchingDroit"));
        Assert.False(result.Any(d => d.Reference == "NotMatchingDroit"));
    }
    
    [Fact]
    public void BuildQuery_WithSalvageSearchFieldsWhereSomeFieldsAreNull_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new DroitSearchForm
        {
            ServicesEstimatedCostFrom = 2, SalvageClaimAwardedTo = 5
        };
        var salvor = new Salvor() {};
        var droits = new List<Droit>
        {
            new()
            {
                Id = Guid.NewGuid(), Reference = "MatchingDroit",
                ServicesEstimatedCost = 10, SalvageClaimAwarded = 4,
                Salvor = salvor
            },
            new()
            {
                Id = Guid.NewGuid(), Reference = "NotMatchingDroit",
                ServicesEstimatedCost = 1,SalvageClaimAwarded = 7,
                Salvor = salvor
            },
            new()
            {
                Id = Guid.NewGuid(), Reference = "OnlyPartialMatchingDroit",
                ServicesEstimatedCost = 10, SalvageClaimAwarded = 6,
                Salvor = salvor
            },
        }.AsQueryable();
    
        // Act
        var result = DroitQueryBuilder.BuildQuery(form, droits, false);
    
        // Assert
        Assert.Equal(1, result.Count()); 
        Assert.True(result.Any(d => d.Reference == "MatchingDroit"));
        Assert.False(result.Any(d => d.Reference == "NotMatchingDroit"));
        Assert.False(result.Any(d => d.Reference == "OnlyPartialMatchingDroit"));
    }

    [Fact]
    public void BuildQuery_WithSalvageSearchBooleanFields_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new DroitSearchForm
        {
            SalvageAwardClaimed = true,MmoLicenceRequired = true, MmoLicenceProvided = false
        };
        var droits = new List<Droit>
        {
            new()
            {
                Id = Guid.NewGuid(), Reference = "MatchingDroit",
                SalvageAwardClaimed = true, MmoLicenceRequired = true, MmoLicenceProvided = false
            },
            new()
            {
                Id = Guid.NewGuid(), Reference = "NotMatchingDroit",
                SalvageAwardClaimed = true, MmoLicenceRequired = true, MmoLicenceProvided = true
            },
        }.AsQueryable();
    
        // Act
        var result = DroitQueryBuilder.BuildQuery(form, droits, false);
    
        // Assert
        Assert.Equal(1, result.Count()); 
        Assert.True(result.Any(d => d.Reference == "MatchingDroit"));
        Assert.False(result.Any(d => d.Reference == "NotMatchingDroit"));
    }
    
    [Fact]
    public void BuildQuery_WithLegacySearchFields_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new DroitSearchForm
        {
            District = "Test" , LegacyFileReference = "Test", GoodsDischargedBy = "Test",
            DateDelivered = "Test", Agent = "Test", RecoveredFromLegacy = "Test", ImportedFromLegacy = true
        };
        var droits = new List<Droit>
        {
            new()
            {
                Id = Guid.NewGuid(), Reference = "MatchingDroit",
                District = "Test" , LegacyFileReference = "Test", GoodsDischargedBy = "Test",
                DateDelivered = "Test", Agent = "Test", RecoveredFromLegacy = "Test", ImportedFromLegacy = true,
            },
            new()
            {
                Id = Guid.NewGuid(), Reference = "PartiallyMatchingDroit",
                District = "TestAndStuff" , LegacyFileReference = "Test", GoodsDischargedBy = "Test",
                DateDelivered = "Test", Agent = "TestAndStuff", RecoveredFromLegacy = "Test", ImportedFromLegacy = true,
            },
            new()
            {
                Id = Guid.NewGuid(), Reference = "NotMatchingDroit",
            },
        }.AsQueryable();
    
        // Act
        var result = DroitQueryBuilder.BuildQuery(form, droits, false);
    
        // Assert
        Assert.Equal(2, result.Count()); 
        Assert.True(result.Any(d => d.Reference == "MatchingDroit"));
        Assert.True(result.Any(d => d.Reference == "PartiallyMatchingDroit"));
        Assert.False(result.Any(d => d.Reference == "NotMatchingDroit"));
    }
}