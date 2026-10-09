using Droits.Controllers;
using Droits.Models.Entities;
using Droits.Models.ViewModels;
using Droits.Models.ViewModels.ListViews;
using Droits.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Droits.Tests.UnitTests.Controllers;

public class SalvorControllerUnitTests
{
    [Fact]
    public async Task View_LoadsPagedDroitsAndPreservesSelectedTab()
    {
        var salvorId = Guid.NewGuid();
        var searchOptions = new SearchOptions { PageNumber = 2, PageSize = 10 };
        var pagedDroits = new DroitListView { PageNumber = 2, PageSize = 10, TotalCount = 11 };
        var salvorService = new Mock<ISalvorService>();
        var droitService = new Mock<IDroitService>();
        salvorService
            .Setup(service => service.GetSalvorAsync(salvorId))
            .ReturnsAsync(new Salvor { Id = salvorId, Name = "Test Salvor" });
        droitService
            .Setup(service => service.GetSalvorDroitsListViewAsync(salvorId, searchOptions))
            .ReturnsAsync(pagedDroits);
        var controller = new SalvorController(
            Mock.Of<ILogger<SalvorController>>(),
            salvorService.Object,
            droitService.Object);

        var result = await controller.View(salvorId, searchOptions, "droits");

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<SalvorView>(viewResult.Model);
        Assert.Same(pagedDroits, model.Droits);
        Assert.Equal("droits", viewResult.ViewData["SelectedTab"]);
        droitService.Verify(service => service.GetSalvorDroitsListViewAsync(salvorId, searchOptions), Times.Once);
    }
}