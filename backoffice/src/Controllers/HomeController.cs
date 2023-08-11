using System.Diagnostics;
using Droits.Models.ViewModels;
using Droits.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Droits.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IDroitService _droitService;


    public HomeController(ILogger<HomeController> logger, IDroitService droitService)
    {
        _logger = logger;
        _droitService = droitService;
    }


    public async Task<IActionResult> Index(SearchOptions searchOptions)
    {
        searchOptions.IncludeAssociations = true;
        searchOptions.FilterByAssignedUser = true;
        
        var model = await _droitService.GetDroitsListViewAsync(searchOptions);
        return View(model);
    }


    public IActionResult Privacy()
    {
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorView
            { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}