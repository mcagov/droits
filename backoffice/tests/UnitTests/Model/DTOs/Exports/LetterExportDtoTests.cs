using Droits.Models.DTOs.Exports;
using Droits.Models.Entities;

namespace Droits.Tests.UnitTests.Model.DTOs.Exports;

public class LetterExportDtoTests
{
    public LetterExportDtoTests()
    {
        
    }


    [Fact]
    public void LetterDto_WithAGivenLetter_ReturnsCorrectInformation()
    {
        // Assemble
        var qaUser = new ApplicationUser()
        {
            Id = Guid.NewGuid(),
            Name = "RoW"
        };
        var droit = new Droit()
        {
            Id = Guid.NewGuid(),
            Reference = "111/11"
        };
        var letter = new Letter()
        {
            Id = Guid.NewGuid(),
            QualityApprovedUser = qaUser,
            Recipient = "Test",
            Droit = droit,
            SenderUserId = Guid.NewGuid(),
            DateSent = DateTime.Now,
        };
        // Act
        var letterDto = new LetterExportDto(letter);
        // Assert
        Assert.Equal(letter.Recipient,letterDto.Recipient);
        Assert.Equal(letter.QualityApprovedUser.Name,letterDto.QualityApprovedUser);
        Assert.Equal(letter.Droit.Reference,letterDto.DroitReference);
        Assert.Equal(letter.Status,letterDto.Status);
        Assert.Equal(letter.Type,letterDto.Type);

    }
}