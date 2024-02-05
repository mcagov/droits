using Droits.Models.DTOs;
using Droits.Models.DTOs.Powerapps;
using Droits.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Droits.Controllers;
public class MigrationController : Controller
{
    private readonly ILogger<MigrationController> _logger;

    private readonly IMigrationService _service;

    public MigrationController(ILogger<MigrationController> logger, IMigrationService migrationService)
    {
        _logger = logger;
        _service = migrationService;
    }

    public IActionResult Index()
    {
        return RedirectToAction("Index", "Home");
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
    public async Task<IActionResult> MigrateWreck([FromBody] PowerappsWreckDto request)
    {

        try
        {
            var savedWreck = await _service.MigrateWreckAsync(request);
            
            return Json(new
            {
                wreck = new
                {
                    savedWreck.Id,
                    savedWreck.PowerappsWreckId,
                    savedWreck.Name
                }
            });
        }
        catch ( Exception e )
        {
            _logger.LogError("Wreck could not be saved" + e);
            return NotFound();
        }

    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> MigrateNote([FromBody] PowerappsNoteDto request)
    {

        try
        {
            var savedNote = await _service.MigrateNoteAsync(request);
            
            return Json(new
            {
                note = new
                {
                    savedNote.Id,
                    savedNote.WreckId,
                    savedNote.DroitId,
                    savedNote.SalvorId,
                    savedNote.LetterId
                }
            });
        }
        catch ( Exception e )
        {
            _logger.LogError("Note could not be saved" + e);
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
    public async Task<IActionResult> MigrateWreckMaterial(
        [FromBody] PowerappsWreckMaterialDto request)
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
}