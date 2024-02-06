using Droits.Exceptions;
using Droits.Helpers;
using Droits.Models.DTOs;
using Droits.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Droits.Controllers;
public class ApiController : Controller
{
    private readonly ILogger<ApiController> _logger;

    private readonly IApiService _service;
    private readonly IConfiguration _configuration;


    public ApiController(ILogger<ApiController> logger, IApiService apiService, IConfiguration configuration)
    {
        _logger = logger;
        _service = apiService;
        _configuration = configuration;

    }

    public IActionResult Index()
    {
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Send([FromBody] SubmittedReportDto report, [FromHeader(Name = "X-API-Key")] string apiKey)
    {
        if (!RequestHelper.IsValidApiKey(apiKey, _configuration))
        {
            return Unauthorized("Invalid API key");
        }
        
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
    public async Task<IActionResult> Salvor(string email, [FromHeader(Name = "X-API-Key")] string apiKey)
    {
        if (!RequestHelper.IsValidApiKey(apiKey, _configuration))
        {
            return Unauthorized("Invalid API key");
        }
        
        try
        {

            var salvorInfo = await _service.GetSalvorInfoAsync(email);

            return Json(salvorInfo);
        }
        catch ( SalvorNotFoundException )
        {
            return NotFound("Salvor not found");
        }

    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Droit(Guid id, string? salvorId, [FromHeader(Name = "X-API-Key")] string apiKey)
    {
        if (!RequestHelper.IsValidApiKey(apiKey, _configuration))
        {
            return Unauthorized("Invalid API key");
        }
        
        var report = await _service.GetReportByIdAsync(id);

        if ( report.SalvorId != salvorId )
        {
            return Unauthorized("Unauthorized Salvor");
        }
        
        return Json(report);
        
    }
}