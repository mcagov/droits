using Droits.Models.Entities;
using Droits.Models.ViewModels;

namespace Droits.Tests.UnitTests.Model.ViewModels;

public class LetterPersonalisationViewUnitTests
{
    private readonly Droit _droit;
    private readonly Droit _droitPlural;
    
    public LetterPersonalisationViewUnitTests()
    {
        _droit = new Droit()
        {
            Reference = "111/11",
            ReportedDate = new DateTime(1971, 7, 14),
            Wreck = new Wreck() 
            {
                Name = "Sinky"
            },
            WreckMaterials = 
            [
                new WreckMaterial()
                {
                    Name = "Rusty Tap"
                }
            ],
            Salvor = new Salvor()
            {
                Name = "Mr Salvor"
            }
        };
        
        _droitPlural = new Droit()
        {
            WreckMaterials = 
            [
                new WreckMaterial()
                {
                    Name = "Oar"
                },
                new WreckMaterial()
                {
                    Name = "Gold"
                }
            ]
        };
    }
    
    [Fact]
    public void ShouldReturnCorrectLetterContent()
    {
        var letterView = new LetterPersonalisationView(_droit);
        var response = letterView.SubstituteContent("((reference)) ((wreck)) ((foobar)) ((date)) ((full name)) ((items))");

        Assert.Equal("111/11 Sinky ((foobar)) 14/07/1971 Mr Salvor * Rusty Tap", response);
    }
    
    [Fact]
    public void ShouldReturnCorrectSingularContent()
    {
        var letterView = new LetterPersonalisationView(_droit);
        var response = letterView.SubstituteContent("((item pluralised)) ((has pluralised)) ((this pluralised)) ((is pluralised)) ((piece pluralised))");

        Assert.Equal("item has this is piece", response);
    }
    
    [Fact]
    public void ShouldReturnCorrectPluralContent()
    {
        var letterView = new LetterPersonalisationView(_droitPlural);
        var response = letterView.SubstituteContent("((item pluralised)) ((has pluralised)) ((this pluralised)) ((is pluralised)) ((piece pluralised))");

        Assert.Equal("items have these are pieces", response);
    }
    
    [Fact]
    public void ShouldReturnListOfMultipleItems()
    {
        var letterView = new LetterPersonalisationView(_droitPlural);
        var response = letterView.SubstituteContent("((items))");

        Assert.Equal("* Oar\n* Gold", response);
    }
    
    [Fact]
    public void ShouldCapitalizePluralisedWordAtStartOfSentence()
    {
        var letterView = new LetterPersonalisationView(_droitPlural);
        var response = letterView.SubstituteContent
            (
                "((item pluralised)) described by you. " +
                "((item pluralised)) described by you: " +
                "((item pluralised)) described by you? " +
                "((item pluralised)) described by you! " +
                "The ((item pluralised)) described by you. " +
                "The Receiver of Wreck's responsibility for ((this pluralised)) ((item pluralised)) ((is pluralised)) now discharged. " +
                "As discussed, ((this pluralised)) ((is pluralised)) now discharged. " +
                "There is something in the water (treasure?) ((this pluralised)) ((is pluralised)) now discharged." +
                "There is something in the water (treasure)? ((this pluralised)) ((is pluralised)) now discharged."
            );

        Assert.Equal
            (                
                "Items described by you. " +
                "Items described by you: " +
                "Items described by you? " +
                "Items described by you! " +
                "The items described by you. " +
                "The Receiver of Wreck's responsibility for these items are now discharged. " +
                "As discussed, these are now discharged. " +
                "There is something in the water (treasure?) These are now discharged." +
                "There is something in the water (treasure)? These are now discharged."
            , response);
    }
    
    [Fact]
    public void ShouldReturnCorrectLetterContentForDuplicates()
    {
        var letterView = new LetterPersonalisationView(_droit);
        var response = letterView.SubstituteContent("((reference)) ((reference))");

        Assert.Equal("111/11 111/11", response);
    }
}