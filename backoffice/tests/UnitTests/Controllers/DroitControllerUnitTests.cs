using Droits.Controllers;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;

namespace Droits.Tests.UnitTests.Controllers;

public class DroitControllerUnitTests
{
    private readonly DroitController _controller;
    private readonly Mock<ITempDataDictionary> _mockTempData;
    
    public DroitControllerUnitTests()
    {
        var mockLogger = new Mock<ILogger<DroitController>>();
        var mockDroitService = new Mock<IDroitService>();
        var mockWreckService = new Mock<IWreckService>();
        var mockSalvorService = new Mock<ISalvorService>();
        var mockUserService = new Mock<IUserService>();
        var mockHttpContext = new DefaultHttpContext();
        _mockTempData = new Mock<ITempDataDictionary>();
        
        mockWreckService
            .Setup(s => s.GetWrecksAsync())
            .ReturnsAsync([
                new Wreck { Name = "Wreck 1" },
                new Wreck { Name = "Wreck 2" }
            ]);
        mockSalvorService
            .Setup(s => s.GetSalvorsAsync())
            .ReturnsAsync([
                new Salvor { Name = "Salvor 1" },
                new Salvor { Name = "Salvor 2" }
            ]);

        _controller =
            new DroitController(mockLogger.Object, mockDroitService.Object,
                mockWreckService.Object, mockSalvorService.Object,
                mockUserService.Object)
            {
                TempData = _mockTempData.Object
            };
        
        _controller.ControllerContext.HttpContext = mockHttpContext;
    }
    
    [Fact]
    public async Task Save_NullForm_ReturnsBadRequest()
    {
        // Given
        DroitForm? form = null;

        // When
        var result = await _controller.Save(form);

        // Then
       Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async void Save_ModelStateInvalid_ReturnsEditViewWithForm()
    {
        // Given
        var form = new DroitForm();
        
        _controller.ModelState.AddModelError("error", "some error");
        _mockTempData.Setup(t => t["ErrorMessage"]).Returns("Could not save Droit");
        
        // When
        var result = await _controller.Save(form);

        // Then
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Edit", viewResult.ViewName);
        var model = Assert.IsAssignableFrom<DroitForm>(viewResult.ViewData.Model);
        Assert.Equal(form, model);
    }
    
}