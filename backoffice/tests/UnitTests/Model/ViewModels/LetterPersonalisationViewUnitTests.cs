using Droits.Models.Entities;
using Droits.Models.ViewModels;

namespace Droits.Tests.UnitTests.Model.ViewModels;

public class LetterPersonalisationViewUnitTests
{
    private readonly Droit _droit;
    public LetterPersonalisationViewUnitTests()
    {
        _droit = new Droit()
        {
            Reference = "111/11",
            ReportedDate = new DateTime(1971, 7, 14),
            Wreck = new Wreck() 
            {
                Name = "Sinky"
            }
        };

    }
    [Fact]
    public void ShouldReturnCorrectLetterContent()
    {
        var letterView = new LetterPersonalisationView(_droit);
        var response = letterView.SubstituteContent("((reference)) ((wreck)) ((foobar)) ((date))");

        Assert.Equal("111/11 Sinky ((foobar)) 14/07/1971", response);
    }
    [Fact]
    public void ShouldReturnCorrectLetterContentForDuplicates()
    {
        var letterView = new LetterPersonalisationView(_droit);
        var response = letterView.SubstituteContent("((reference)) ((reference))");

        Assert.Equal("111/11 111/11", response);
    }
}