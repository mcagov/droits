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
        
        var content = """
                      Droit reference: ((reference))
                      Wreck name: ((wreck))
                      Invalid personalisation: ((foobar))
                      Reported date: ((date))
                      Salvor name: ((full name))
                      List of wreck items:
                      ((items))
                      """;
        
        var expectedContent = """
                              Droit reference: 111/11
                              Wreck name: Sinky
                              Invalid personalisation: ((foobar))
                              Reported date: 14/07/1971
                              Salvor name: Mr Salvor
                              List of wreck items:
                              * Rusty Tap
                              """;
        
        var response = letterView.SubstituteContent(content);

        Assert.Equal(expectedContent, response);
    }
    
    [Fact]
    public void ShouldReturnCorrectSingularContent()
    {
        var letterView = new LetterPersonalisationView(_droit);

        var content = """
                      The ((item pluralised)) ((has pluralised)) been received.
                      Please look after ((this pluralised)) ((piece pluralised)).
                      """;
        
        var expectedContent = """
                              The item has been received.
                              Please look after this piece.
                              """;
        
        var response = letterView.SubstituteContent(content);

        Assert.Equal(expectedContent, response);
    }
    
    [Fact]
    public void ShouldReturnCorrectPluralContent()
    {
        var letterView = new LetterPersonalisationView(_droitPlural);
        
        var content = """
                      The ((item pluralised)) ((has pluralised)) been received.
                      Please look after ((this pluralised)) ((piece pluralised)).
                      """;
        
        var expectedContent = """
                              The items have been received.
                              Please look after these pieces.
                              """;
        
        var response = letterView.SubstituteContent(content);

        Assert.Equal(expectedContent, response);
    }
    
    [Fact]
    public void ShouldReturnListOfMultipleItems()
    {
        var letterView = new LetterPersonalisationView(_droitPlural);
        
        var content = """
                      List of wreck items:
                      ((items))
                      """;
        
        var expectedContent = """
                              List of wreck items:
                              * Oar
                              * Gold
                              """;
        
        var response = letterView.SubstituteContent(content);

        Assert.Equal(expectedContent, response);
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