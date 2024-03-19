using Droits.Helpers.SearchHelpers;
using Droits.Models.Entities;
using Droits.Models.FormModels.SearchFormModels;
using Xunit.Abstractions;

namespace Droits.Tests.UnitTests.Helpers.SearchHelpers;

public class SalvorQueryBuilderUnitTests
{
        
    private readonly ITestOutputHelper _output;


    public SalvorQueryBuilderUnitTests(ITestOutputHelper output)
    {
        _output = output;
    }
    
    [Fact]
    public void BuildQuery_WithValidName_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new SalvorSearchForm { Name = "Test" };
        var salvors = new List<Salvor>
        {
            new() { Id = Guid.NewGuid(), Name = "TestSalvor" },
            new() { Id = Guid.NewGuid(), Name = "AnotherTestSalvor" },
            new() { Id = Guid.NewGuid(), Name = "NotMatching" }
        }.AsQueryable();

        // Act
        var result = SalvorQueryBuilder.BuildQuery(form, salvors, false);

        // Assert
        Assert.Equal(2, result.Count()); 
        Assert.True(result.All(s => s.Name.ToLower().Contains("test")));
    }

    [Fact]
    public void BuildQuery_WithNullOrEmptyName_ReturnsOriginalQuery()
    {
        // Arrange
        var form = new SalvorSearchForm { Name = null };
        var salvors = new List<Salvor>
        {
            new() { Id = Guid.NewGuid(), Name = "TestSalvor" },
            new() { Id = Guid.NewGuid(), Name = "AnotherTestSalvor" }
        }.AsQueryable();

        // Act
        var result = SalvorQueryBuilder.BuildQuery(form, salvors, false);

        // Assert
        Assert.Equal(salvors.Count(), result.Count());
        Assert.Equal(salvors.ToList(), result.ToList());
    }
    
    [Fact]
    public void BuildQuery_WithValidFuzzyName_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new SalvorSearchForm { Name = "TustSulvor" };
        var salvors = new List<Salvor>
        {
            new() { Id = Guid.NewGuid(), Name = "TestSalvor" },
            new() { Id = Guid.NewGuid(), Name = "Test Salvor Sr" },
            new() { Id = Guid.NewGuid(), Name = "Test Salvor NotMatching" },
        }.AsQueryable();

            
        _output.WriteLine($"{form.Name.ToLower()} Len: {form.Name.Length} Threshold: {SearchHelper.GetLevenshteinDistanceThreshold(form.Name.ToLower())}");
        foreach (var s in salvors)
        {
            _output.WriteLine($"{s.Name.ToLower()} Distance: {SearchHelper.GetLevenshteinDistance(form.Name.ToLower(), s.Name.ToLower())}");
        }

        
        // Act
        var result = SalvorQueryBuilder.BuildQuery(form, salvors, false);

        // Assert
        Assert.Equal(2, result.Count()); 
        Assert.Equal("TestSalvor",result.First().Name);
    }
    
    [Fact]
    public void BuildQuery_WithValidEmail_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new SalvorSearchForm { Email = "Salvor@email.com" };
        var salvors = new List<Salvor>
        {
            new() { Id = Guid.NewGuid(), Email = "TestSalvor@email.com" },
            new() { Id = Guid.NewGuid(), Email = "AnotherTestSalvor@email.com" },
            new() { Id = Guid.NewGuid(), Email = "NotMatching@email.com" }
        }.AsQueryable();

        // Act
        var result = SalvorQueryBuilder.BuildQuery(form, salvors, false);

        // Assert
        Assert.Equal(2, result.Count()); 
        Assert.True(result.All(s => s.Email.ToLower().Contains("test")));
    }
    
}