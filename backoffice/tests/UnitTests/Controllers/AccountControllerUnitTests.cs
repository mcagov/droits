using Droits.Controllers;
using Droits.Models.FormModels.SearchFormModels;
using Droits.Models.ViewModels;
using Droits.Models.ViewModels.ListViews;
using Droits.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Droits.Tests.UnitTests.Controllers;

public class AccountControllerUnitTests
{
    [Fact]
    public async Task Index_PreservesDashboardSortOptions()
    {
        var droitService = new Mock<IDroitService>();
        var letterService = new Mock<ILetterService>();
        droitService
            .Setup(service => service.GetDroitsListViewAsync(It.IsAny<SearchOptions>()))
            .ReturnsAsync(new DroitListView());
        letterService
            .Setup(service => service.GetApprovedUnsentLettersListViewForCurrentUserAsync(
                It.IsAny<SearchOptions>()))
            .ReturnsAsync(new LetterListView());

        var controller = new AccountController(
            Mock.Of<ILogger<AccountController>>(),
            droitService.Object,
            Mock.Of<IAccountService>(),
            letterService.Object);
        var dashboardSearchForm = new DashboardSearchForm
        {
            OrderColumn = "Status",
            OrderDescending = false
        };

        var result = await controller.Index(new DashboardView
        {
            DashboardSearchForm = dashboardSearchForm
        });

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<DashboardView>(viewResult.Model);
        Assert.Equal("Status", model.DashboardSearchForm.OrderColumn);
        Assert.False(model.DashboardSearchForm.OrderDescending);
    }
}
