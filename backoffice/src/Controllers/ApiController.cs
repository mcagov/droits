#region

using Droits.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

#endregion

namespace Droits.Controllers;

public class ApiController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Random _random;


    public ApiController(ILogger<HomeController> logger)
    {
        _logger = logger;
        _random = new Random();
    }


    public IActionResult Index()
    {
        return Json(new { status = "Got here..." });
    }


    [HttpPost]
    public async Task<IActionResult> Send()
    {
        using ( var reader = new StreamReader(Request.Body) )
        {
            var body = await reader.ReadToEndAsync();
            var report = JsonConvert.DeserializeObject<WreckReport>(body);

            if ( report == null ) return NotFound();

            // Generate reference for report:
            report.Reference = $"{_random.NextInt64(1, 100)}/{DateTime.UtcNow.ToString("yy")}";

            return Json
            (
                new
                {
                    reference = report.Reference,
                    report,
                    status = "Accepted"
                }
            );
        }
    }
}