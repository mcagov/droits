using Bogus.DataSets;
using Droits.Models.DTOs;
using Droits.Models.DTOs.Powerapps;
using Droits.Models.Enums;
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
            var savedWrecks = await _service.MigrateWrecksAsync(request);

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
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> MigrateDroits([FromBody] PowerappsDroitReportsDto request)
    {

        try
        {
            var savedDroits = await _service.MigrateDroitsAsync(request);

            var droitIdsList = savedDroits.Select(d => new
            {
                d.Id,
                d.PowerappsDroitId,
                d.PowerappsWreckId,
                d.Reference
            }).ToList();

            return Json(new
            {
                droits = droitIdsList
            });
        }
        catch ( Exception e )
        {
            _logger.LogError("Droits could not be saved" + e);
            return NotFound();
        }

    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> MigrateDroit([FromBody] PowerappsDroitReportDto request)
    {

        try
        {
            var savedDroit = await _service.MigrateDroitAsync(request);
            
            return Json(new
            {
                droit = new
                {
                    savedDroit.Id,
                    savedDroit.PowerappsDroitId,
                    savedDroit.PowerappsWreckId,
                    savedDroit.Reference
                }
            });
        }
        catch ( Exception e )
        {
            _logger.LogError("Droits could not be saved" + e);
            return NotFound();
        }

    }
    
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> MigrateWreckMaterial([FromBody] PowerappsWreckMaterialDto request)
    {
    
        try
        {
            var savedWreckMaterial = await _service.MigrateWreckMaterialAsync(request);
            
            return Json(new
            {
                WreckMaterial = new
                {
                    savedWreckMaterial.Id,
                    savedWreckMaterial.DroitId,
                    savedWreckMaterial.Name
                }
            });
        }
        catch ( Exception e )
        {
            _logger.LogError("WM could not be saved" + e);
            return NotFound();
        }
    
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Test(string email)
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
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetFirstWMImage(Guid id)
    {

        var wm = await _service.GetWreckMaterialAsync(id);

        var firstImage = wm.Images.FirstOrDefault().Id.ToString();

        return Redirect($"http://localhost:5000/Image/DisplayImage/{firstImage}");

    }

    
}