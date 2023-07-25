using Droits.Models.Entities;
using Droits.Models.ViewModels;

namespace Droits.Tests.UnitTests.Letter;

public class LetterPersonalisationViewUnitTests
{
    //Add tests here for SubstituteLetterContentWithParamsAsync// SamB
    [Fact]
    public async void SubstituteLetterContentWithParamsAsync_ShouldReturnCorrectLetterContent()
    {
        Wreck wreck = new()
        {
            Name = "Sinky"
        };
        Droit droit = new()
        {
            Reference = "111/11",
            ReportedDate = new DateTime(1971,7,14),
            Wreck = wreck
        };

        var letterView = new LetterPersonalisationView(droit);
        var response = letterView.SubstituteContent("((reference)) ((wreck)) ((foobar)) ((date))");
        
        Assert.Equal("111/11 Sinky ((foobar)) 14/07/1971",response);
    }

}