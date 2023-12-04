using Bogus.DataSets;
using Droits.Models.DTOs;
using Droits.Models.DTOs.Powerapps;
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
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> MigrateWrecks([FromBody] PowerappsWrecksDto request)
    {

        try
        {
            var savedWrecks = await _service.SaveWrecksAsync(request);

            var wreckIdsList = savedWrecks.Select(w => new
            {
                w.Id,
                w.PowerappsWreckId,
                w.Name
            }).ToList();

            return Json(new
            {
                wrecks = wreckIdsList
            });
        }
        catch ( Exception e )
        {
            _logger.LogError("Wrecks could not be saved" + e);
            return NotFound();
        }

    }
    
    
}