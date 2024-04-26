using Droits.Exceptions;
using Droits.Helpers;
using Droits.Models.DTOs;
using Droits.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Timeouts;
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
    [RequestTimeout(int.MaxValue)]
    public async Task<IActionResult> SubmitDroit([FromBody] SubmittedReportDto report, [FromHeader(Name = "X-API-Key")] string apiKey)
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
                    droitId = savedDroit.Id,
                    salvorId = savedDroit.SalvorId
                }
            );
        }
        catch ( Exception e )
        {
            _logger.LogError("Droit could not be saved" + e);
            return NotFound();
        }

    }
    
    [HttpPost]
    [AllowAnonymous]
    [RequestTimeout(int.MaxValue)]
    public async Task<IActionResult> SubmitWreckMaterial([FromBody] SubmittedWreckMaterialDto wreckMaterialReport, [FromHeader(Name = "X-API-Key")] string apiKey)
    {
        if (!RequestHelper.IsValidApiKey(apiKey, _configuration))
        {
            return Unauthorized("Invalid API key");
        }
        
        try
        {
            var savedWm = await _service.SaveWreckMaterialReportAsync(wreckMaterialReport);

            if ( savedWm == null )
            {
                throw new WreckMaterialNotFoundException("Wreck material could not be saved.");
            }
            return Json
            (
                new
                {
                    wreckMaterialId = savedWm.Id,
                    droitId = savedWm.DroitId
                }
            );
        }
        catch ( Exception e )
        {
            _logger.LogError($"Wreck material could not be saved. - {e}");
            return NotFound();
        }

    }
 
    
        [HttpPost]
        [AllowAnonymous]
        [RequestTimeout(int.MaxValue)]
        public async Task<IActionResult> SendConfirmationEmail([FromBody] Guid droitId, [FromHeader(Name = "X-API-Key")] string apiKey)
        {
            if (!RequestHelper.IsValidApiKey(apiKey, _configuration))
            {
                return Unauthorized("Invalid API key");
            }
            
            try
            {
                await _service.SendConfirmationEmail(droitId);
            }
            catch ( Exception e )
            {
                _logger.LogError($"Confirmation email could not be send for droit {droitId}. - {e}");
                return BadRequest(new { error = $"Error sending Confirmation email - {e.Message}" });
            }

            return Ok();
        }
     
        
    [HttpGet]
    [AllowAnonymous]
    [RequestTimeout(int.MaxValue)]
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
    [RequestTimeout(int.MaxValue)]
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