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
            new() { Id = Guid.NewGuid(), Name = "TestWreck44" },
            new() { Id = Guid.NewGuid(), Name = "TestWreck555" },
            new() { Id = Guid.NewGuid(), Name = "TestWreck6666" },
        }.AsQueryable();

        // Act
        var result = WreckQueryBuilder.BuildQuery(form, wrecks, false);

        // Assert
        Assert.Equal(1, result.Count()); 
        Assert.Equal("TestWreck44",result.First().Name);
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
        Assert.True(result.All(w => w.OwnerName.ToLower().Contains("test")));
    }

    [Fact]
    public void BuildQuery_WithNullOrEmptyOwnerName_ReturnsOriginalQuery()
    {
        // Arrange
        var form = new WreckSearchForm { OwnerName = null };
        var wrecks = new List<Wreck>
        {
            new() { Id = Guid.NewGuid(), OwnerName = "TestOwner" },
            new() { Id = Guid.NewGuid(), OwnerName = "AnotherTestOwner" }
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
            new() { Id = Guid.NewGuid(), Name = "TestOwner44" },
            new() { Id = Guid.NewGuid(), Name = "TestOwner555" },
            new() { Id = Guid.NewGuid(), Name = "TestOwner6666" },
        }.AsQueryable();

        // Act
        var result = WreckQueryBuilder.BuildQuery(form, wrecks, false);

        // Assert
        Assert.Equal(1, result.Count()); 
        Assert.Equal("TestOwner44",result.First().Name);
    }
    
}