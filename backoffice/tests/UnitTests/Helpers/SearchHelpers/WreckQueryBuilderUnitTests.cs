using Droits.Helpers.SearchHelpers;
using Droits.Models.Entities;
using Droits.Models.FormModels.SearchFormModels;

namespace Droits.Tests.UnitTests.Helpers.SearchHelpers;

public class WreckQueryBuilderUnitTests
{
    [Fact]
    public void BuildQuery_WithValidName_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new WreckSearchForm { Name = "Test" };
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
    public void BuildQuery_WithNullOrEmptyName_ReturnsOriginalQuery()
    {
        // Arrange
        var form = new WreckSearchForm { Name = null };
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
    public void BuildQuery_WithValidFuzzyName_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new WreckSearchForm { Name = "TustWrack" };
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
    
}