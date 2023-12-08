using Droits.Helpers.SearchHelpers;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Models.FormModels.SearchFormModels;

namespace Droits.Tests.UnitTests.Helpers.SearchHelpers;

public class LetterQueryBuilderUnitTests
{
    [Fact]
    public void BuildQuery_WithValidEmail_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new LetterSearchForm { Recipient = "Letter@email.com" };
        var letters = new List<Letter>
        {
            new() { Id = Guid.NewGuid(), Recipient = "TestLetter@email.com" },
            new() { Id = Guid.NewGuid(), Recipient = "AnotherTestLetter@email.com" },
            new() { Id = Guid.NewGuid(), Recipient = "NotMatching@email.com" }
        }.AsQueryable();

        // Act
        var result = LetterQueryBuilder.BuildQuery(form, letters);

        // Assert
        Assert.Equal(2, result.Count()); 
        Assert.True(result.All(s => s.Recipient.ToLower().Contains("test")));
    }
        
    [Fact]
    public void BuildQuery_WithNullOrEmptyRecipient_ReturnsOriginalQuery()
    {
        // Arrange
        var form = new LetterSearchForm { Recipient = null };
        var letters = new List<Letter>
        {
            new() { Id = Guid.NewGuid(), Recipient = "TestLetter@email.com" },
            new() { Id = Guid.NewGuid(), Recipient = "AnotherTestLetter@email.com" }
        }.AsQueryable();

        // Act
        var result = LetterQueryBuilder.BuildQuery(form, letters);

        // Assert
        Assert.Equal(letters.Count(), result.Count());
        Assert.Equivalent(letters.ToList(), result.ToList());
    }
    
    [Fact]
    public void BuildQuery_WithASingleStatus_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new LetterSearchForm { StatusList = new List<LetterStatus>(){LetterStatus.Sent} };
        var letters = new List<Letter>
        {
            new() { Id = Guid.NewGuid(), Status = LetterStatus.Draft },
            new() { Id = Guid.NewGuid(), Status = LetterStatus.Sent }
        }.AsQueryable();

        // Act
        var result = LetterQueryBuilder.BuildQuery(form, letters);

        // Assert
        Assert.Equal(1, result.Count());
        Assert.Equal(LetterStatus.Sent, result.First().Status);
    }
    
    [Fact]
    public void BuildQuery_WithMultipleStatuses_ReturnsFilteredQuery()
    {
        // Arrange
        var form = new LetterSearchForm { StatusList = new List<LetterStatus>(){LetterStatus.Sent,LetterStatus.Draft} };
        var letters = new List<Letter>
        {
            new() { Id = Guid.NewGuid(), Status = LetterStatus.Draft },
            new() { Id = Guid.NewGuid(), Status = LetterStatus.Sent },
            new() { Id = Guid.NewGuid(), Status = LetterStatus.QCApproved }
        }.AsQueryable();

        // Act
        var result = LetterQueryBuilder.BuildQuery(form, letters);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.True(result.Any(l => l.Status == LetterStatus.Draft));
        Assert.True(result.Any(l => l.Status == LetterStatus.Sent));
        Assert.False(result.Any(l => l.Status == LetterStatus.QCApproved));
    }
    
}