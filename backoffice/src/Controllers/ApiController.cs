using Droits.Models.DTOs;
using Droits.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Droits.Controllers;
public class ApiController : Controller
{
    private readonly ILogger<ApiController> _logger;

    private readonly IApiService _service;

    public ApiController(ILogger<ApiController> logger, IApiService apiService)
    {
        _logger = logger;
        _service = apiService;
    }

    public IActionResult Index()
    {
        return Json(new { status = "Got here..." });
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Send([FromBody] SubmittedReportDto report)
    {
 

        try
        {
            var savedReport = await _service.SaveDroitReportAsync(report);
            
            return Json
            (
                new
                {
                    reference = savedReport.Reference,
                    savedReport,
                    status = "Accepted"
                }
            );
        }
        catch ( Exception e )
        {
            _logger.LogError("Droit could not be saved" + e);
            return NotFound();
        }

    }
}