using Droits.Helpers.Extensions;

namespace Droits.Tests.UnitTests;

public class DateTimeExtensionsTests
{
    [Fact]
    public void IsBetween_WithADateBetweenTwoDatesExclusive_ShouldReturnTrue()
    {
        var date = new DateTime(2023,10,09);
        var from = new DateTime(2023,10,08);
        var to = new DateTime(2023,10,10);
        
        Assert.True(date.IsBetween(from,to));
    }
    
    [Fact]
    public void IsBetween_WithADateBetweenTwoDatesInclusiveOfFromDate_ShouldReturnTrue()
    {
        var date = new DateTime(2023,10,09);
        var from = new DateTime(2023,10,09);
        var to = new DateTime(2023,10,10);
        
        Assert.True(date.IsBetween(from,to));
    }
    
    [Fact]
    public void IsBetween_WithADateBetweenTwoDatesInclusiveOfToDate_ShouldReturnTrue()
    {
        var date = new DateTime(2023,10,09);
        var from = new DateTime(2023,10,08);
        var to = new DateTime(2023,10,09);
        
        Assert.True(date.IsBetween(from,to));
    }
    
    [Fact]
    public void IsBetween_WithADateNotBetweenTwoDates_ShouldReturnFalse()
    {
        var date = new DateTime(2023,10,05);
        var from = new DateTime(2023,10,08);
        var to = new DateTime(2023,10,09);
        
        Assert.False(date.IsBetween(from,to));
    }
    [Fact]
    public void IsBetween_WithADateBetweenNullAndAToDate_ShouldReturnTrue()
    {
        var date = new DateTime(2023,10,05);
        DateTime? from = null;
        var to = new DateTime(2023,10,09);
        
        Assert.True(date.IsBetween(from,to));
    }
    [Fact]
    public void IsBetween_WithADateBetweenAFromDateAndNull_ShouldReturnTrue()
    {
        var date = new DateTime(2023,10,09);
        var from = new DateTime(2023,10,08);
        DateTime? to = null;
        
        Assert.True(date.IsBetween(from,to));
    }
    
    [Fact]
    public void IsBetween_WithADateBetweenNullAndNull_ShouldReturnTrue()
    {
        var date = new DateTime(2023,10,09);
        DateTime? from = null;
        DateTime? to = null;
        
        Assert.True(date.IsBetween(from,to));
    }
    
}