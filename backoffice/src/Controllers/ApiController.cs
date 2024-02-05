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
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Send([FromBody] SubmittedReportDto report)
    {
 

        try
        {
            var savedDroit = await _service.SaveDroitReportAsync(report);
            
            return Json
            (
                new
                {
                    reference = savedDroit.Reference,
                    salvorId = savedDroit.SalvorId,
                    originalSubmission = savedDroit.OriginalSubmission
                }
            );
        }
        catch ( Exception e )
        {
            _logger.LogError("Droit could not be saved" + e);
            return NotFound();
        }

    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Salvor(string email)
    {

        var salvorInfo = await _service.GetSalvorInfoAsync(email);

        return Json(salvorInfo);
        
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Droit(Guid id)
    {

        var report = await _service.GetReportByIdAsync(id);

        return Json(report);
        
    }
}