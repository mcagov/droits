using System.Diagnostics;
using Droits.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Droits.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    
    public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        if (User.Identity is { IsAuthenticated: true })
        {
            return RedirectToAction("Index", "Account");
        }
        
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }
    
    public async Task<IActionResult> FetchMetadata(string endpoint="http://169.254.169.254")
    {
        try
        {
            // Create an HttpClient using the factory
            var httpClient = _httpClientFactory.CreateClient();

            // Make an HTTP GET request to http://169.254.169.254
            var response = await httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var content = await response.Content.ReadAsStringAsync();
                return Content(content, "text/plain");
            }
            else
            {
                // Handle non-success status codes
                return Content($"Error: {response.StatusCode} - {response.ReasonPhrase}", "text/plain");
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions
            return Content($"An error occurred: {ex.Message}", "text/plain");
        }
    }

    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorView
            { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}