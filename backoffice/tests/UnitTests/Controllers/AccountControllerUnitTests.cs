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

    [Fact]
    public async Task Index_WhenApplyingFilters_ResetsDroitsPageAndPreservesLettersPage()
    {
        var droitPageNumber = 0;
        var letterPageNumber = 0;
        var droitService = new Mock<IDroitService>();
        var letterService = new Mock<ILetterService>();
        droitService
            .Setup(service => service.GetDroitsListViewAsync(It.IsAny<SearchOptions>()))
            .Callback<SearchOptions>(options => droitPageNumber = options.PageNumber)
            .ReturnsAsync(new DroitListView());
        letterService
            .Setup(service => service.GetApprovedUnsentLettersListViewForCurrentUserAsync(
                It.IsAny<SearchOptions>()))
            .Callback<SearchOptions>(options => letterPageNumber = options.PageNumber)
            .ReturnsAsync(new LetterListView());

        var controller = new AccountController(
            Mock.Of<ILogger<AccountController>>(),
            droitService.Object,
            Mock.Of<IAccountService>(),
            letterService.Object);

        var result = await controller.Index(new DashboardView
        {
            DashboardSearchForm = new DashboardSearchForm
            {
                ApplyFilters = true,
                DroitsPageNumber = 3,
                LettersPageNumber = 4
            }
        });

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<DashboardView>(viewResult.Model);
        Assert.Equal(1, droitPageNumber);
        Assert.Equal(4, letterPageNumber);
        Assert.Equal(1, model.DashboardSearchForm.DroitsPageNumber);
        Assert.Equal(4, model.DashboardSearchForm.LettersPageNumber);
        Assert.False(model.DashboardSearchForm.ApplyFilters);
    }

    [Fact]
    public async Task Index_WhenNotApplyingFilters_PreservesDroitsPage()
    {
        var droitPageNumber = 0;
        var droitService = new Mock<IDroitService>();
        var letterService = new Mock<ILetterService>();
        droitService
            .Setup(service => service.GetDroitsListViewAsync(It.IsAny<SearchOptions>()))
            .Callback<SearchOptions>(options => droitPageNumber = options.PageNumber)
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

        var result = await controller.Index(new DashboardView
        {
            DashboardSearchForm = new DashboardSearchForm
            {
                DroitsPageNumber = 3,
                LettersPageNumber = 1
            }
        });

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<DashboardView>(viewResult.Model);
        Assert.Equal(3, droitPageNumber);
        Assert.Equal(3, model.DashboardSearchForm.DroitsPageNumber);
    }
}
