using Droits.Helpers.SearchHelpers;
using Droits.Models.Entities;
using Droits.Models.FormModels.SearchFormModels;
using Xunit.Abstractions;

namespace Droits.Tests.UnitTests.Helpers.SearchHelpers;

public class WreckQueryBuilderUnitTests
{
    
    private readonly ITestOutputHelper _output;


    public WreckQueryBuilderUnitTests(ITestOutputHelper output)
    {
        _output = output;
    }
    
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

        _output.WriteLine($"{form.WreckName.ToLower()} Len: {form.WreckName.Length} Threshold: {SearchHelper.GetLevenshteinDistanceThreshold(form.WreckName.ToLower())}");
        foreach (var w in wrecks)
        {
            _output.WriteLine($"{w.Name.ToLower()} Distance: {SearchHelper.GetLevenshteinDistance(form.WreckName.ToLower(), w.Name.ToLower())}");
        }
        
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
            new() { Id = Guid.NewGuid(), Name = "TestWreck Copy" },
            new() { Id = Guid.NewGuid(), Name = "Test Wreck NotMatching" },
        }.AsQueryable();
    
        _output.WriteLine($"{form.WreckName.ToLower()} Len: {form.WreckName.Length} Threshold: {SearchHelper.GetLevenshteinDistanceThreshold(form.WreckName.ToLower())}");
        foreach (var w in wrecks)
        {
            _output.WriteLine($"{w.Name.ToLower()} Distance: {SearchHelper.GetLevenshteinDistance(form.WreckName.ToLower(), w.Name.ToLower())}");
        }
        
        // Act
        var result = WreckQueryBuilder.BuildQuery(form, wrecks, false);
    
        // Assert
        Assert.Equal(2, result.Count()); 
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
        var form = new WreckSearchForm { OwnerName = "TustOwnar" };
        var wrecks = new List<Wreck>
        {
            new() { Id = Guid.NewGuid(), OwnerName = "TestOwner" },
            new() { Id = Guid.NewGuid(), OwnerName = "Test Owner Jr" },
            new() { Id = Guid.NewGuid(), OwnerName = "Test Owner NotMatching" },
        }.AsQueryable();

        _output.WriteLine($"{form.OwnerName.ToLower()} Len: {form.OwnerName.Length} Threshold: {SearchHelper.GetLevenshteinDistanceThreshold(form.OwnerName.ToLower())}");
        foreach (var w in wrecks)
        {
            if ( !string.IsNullOrEmpty(w.OwnerName) )
            {
                _output.WriteLine($"{w.OwnerName.ToLower()} Distance: {SearchHelper.GetLevenshteinDistance(form.OwnerName.ToLower(), w.OwnerName.ToLower())}");
            }
        }
        
        
        // Act
        var result = WreckQueryBuilder.BuildQuery(form, wrecks, false);

        // Assert
        Assert.Equal(2, result.Count()); 
        Assert.Equal("TestOwner",result.First().OwnerName);
    }
    
}