using Droits.Models.Entities;
using Droits.Models.ViewModels;

namespace Droits.Tests.UnitTests.Model.ViewModels;

public class WreckViewUnitTests
{
    private readonly Wreck _wreck;
    public WreckViewUnitTests()
    {
        _wreck = new Wreck()
        {
            DateOfLoss = new DateTime(1640, 1, 1),
        };

    }
    [Fact]
    public void ShouldReturnCorrectDateOfLoss()
    {
        var wreckView = new WreckView(_wreck);
        var response = wreckView.DateOfLoss;
        var expected = new DateTime(1640, 1, 1).ToUniversalTime();

        Assert.Equal(expected, response);
    }
}