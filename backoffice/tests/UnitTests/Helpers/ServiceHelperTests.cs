using System.Linq.Expressions;
using Droits.Helpers;
using Droits.Models.Entities;
using Droits.Models.ViewModels.ListViews;

namespace Droits.Tests.UnitTests.Helpers;

public class ServiceHelperTests
{
    [Fact]
    public void GetOrderColumnExpression_ShouldReturnReportedDate_WhenOrderColumnIsInvalid()
    {
        // Arrange
        var searchOptions = new SearchOptions { OrderColumn = "InvalidColumn" };
        var expectedExpression = (Expression<Func<Droit, object>>)(d => d.ReportedDate);

        // Act
        var result = ServiceHelper.GetOrderColumnExpression(searchOptions);

        // Assert
        Assert.Equal(expectedExpression.ToString(), result.ToString());
    }
    
    [Fact]
    public void GetOrderColumnExpression_ShouldReturnStatus_WhenOrderColumnIsReportedDate()
    {
        // Arrange
        var searchOptions = new SearchOptions { OrderColumn = "ReportedDate" };
        var expectedExpression = (Expression<Func<Droit, object>>)(d => d.ReportedDate);

        // Act
        var result = ServiceHelper.GetOrderColumnExpression(searchOptions);

        // Assert
        Assert.Equal(expectedExpression.ToString(), result.ToString());
    }

    [Fact]
    public void GetOrderColumnExpression_ShouldReturnStatus_WhenOrderColumnIsStatus()
    {
        // Arrange
        var searchOptions = new SearchOptions { OrderColumn = "Status" };
        var expectedExpression = (Expression<Func<Droit, object>>)(d => d.Status);

        // Act
        var result = ServiceHelper.GetOrderColumnExpression(searchOptions);

        // Assert
        Assert.Equal(expectedExpression.ToString(), result.ToString());
    }
}