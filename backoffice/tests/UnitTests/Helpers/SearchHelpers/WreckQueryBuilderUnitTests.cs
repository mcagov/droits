using Droits.Helpers.SearchHelpers;
using Droits.Models.Entities;
using Droits.Models.FormModels.SearchFormModels;

namespace Droits.Tests.UnitTests.Helpers.SearchHelpers;

public class WreckQueryBuilderUnitTests
{
    [Fact]
    public void BuildQuery_WithValidWreckName_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new WreckSearchForm { WreckName = "Test" };
        var wrecks = new List<Wreck>
        {
            new() { Id = Guid.NewGuid(), Name = "TestWreck" },
            new() { Id = Guid.NewGuid(), Name = "AnotherTestWreck" },
            new() { Id = Guid.NewGuid(), Name = "NotMatching" }
        }.AsQueryable();

        // Act
        var result = WreckQueryBuilder.BuildQuery(form, wrecks, false);

        // Assert
        Assert.Equal(2, result.Count()); 
        Assert.True(result.All(w => w.Name.ToLower().Contains("test")));
    }

    [Fact]
    public void BuildQuery_WithNullOrEmptyWreckName_ReturnsOriginalQuery()
    {
        // Arrange
        var form = new WreckSearchForm { WreckName = null };
        var wrecks = new List<Wreck>
        {
            new() { Id = Guid.NewGuid(), Name = "TestWreck" },
            new() { Id = Guid.NewGuid(), Name = "AnotherTestWreck" }
        }.AsQueryable();

        // Act
        var result = WreckQueryBuilder.BuildQuery(form, wrecks, false);

        // Assert
        Assert.Equal(wrecks.Count(), result.Count());
        Assert.Equal(wrecks.ToList(), result.ToList());
    }
    
    [Fact]
    public void BuildQuery_WithValidFuzzyWreckName_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new WreckSearchForm { WreckName = "TustWrack" };
        var wrecks = new List<Wreck>
        {
            new() { Id = Guid.NewGuid(), Name = "TestWreck" },
            new() { Id = Guid.NewGuid(), Name = "TestWreck11" },
            new() { Id = Guid.NewGuid(), Name = "TestWreck222" },
        }.AsQueryable();

        // Act
        var result = WreckQueryBuilder.BuildQuery(form, wrecks, false);

        // Assert
        Assert.Equal(1, result.Count()); 
        Assert.Equal("TestWreck",result.First().Name);
    }
    
    [Fact]
    public void BuildQuery_WithValidOwnerName_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new WreckSearchForm { OwnerName = "Test" };
        var wrecks = new List<Wreck>
        {
            new() { Id = Guid.NewGuid(), OwnerName = "TestOwner" },
            new() { Id = Guid.NewGuid(), OwnerName = "AnotherTestOwner" },
            new() { Id = Guid.NewGuid(), OwnerName = "NotMatching" }
        }.AsQueryable();

        // Act
        var result = WreckQueryBuilder.BuildQuery(form, wrecks, false);

        // Assert
        Assert.Equal(2, result.Count()); 
        Assert.True(result.All(w => w.OwnerName!= null && w.OwnerName.ToLower().Contains("test")));
    }

    [Fact]
    public void BuildQuery_WithNullOrEmptyOwnerName_ReturnsOriginalQuery()
    {
        // Arrange
        var form = new WreckSearchForm { OwnerName = null };
        var wrecks = new List<Wreck>
        {
            new() { Id = Guid.NewGuid(), OwnerName = "TestOwner" },
            new() { Id = Guid.NewGuid(), OwnerName = "TestOwner1" }
        }.AsQueryable();

        // Act
        var result = WreckQueryBuilder.BuildQuery(form, wrecks, false);

        // Assert
        Assert.Equal(wrecks.Count(), result.Count());
        Assert.Equal(wrecks.ToList(), result.ToList());
    }
    
    [Fact]
    public void BuildQuery_WithValidFuzzyOwnerName_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new WreckSearchForm { WreckName = "TustOwnar" };
        var wrecks = new List<Wreck>
        {
            new() { Id = Guid.NewGuid(), Name = "TestOwner" },
            new() { Id = Guid.NewGuid(), Name = "TestOwner11" },
            new() { Id = Guid.NewGuid(), Name = "TestOwner222" },
        }.AsQueryable();

        // Act
        var result = WreckQueryBuilder.BuildQuery(form, wrecks, false);

        // Assert
        Assert.Equal(1, result.Count()); 
        Assert.Equal("TestOwner",result.First().Name);
    }
    
}