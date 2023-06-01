using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Droits.Models;

namespace Droits.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
        // return RedirectToAction("Index","Droit",new {});
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Droits()
    {
        return new OkResult();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
