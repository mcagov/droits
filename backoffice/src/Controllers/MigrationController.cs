using System.Globalization;
using CsvHelper;
using Droits.Helpers;
using Droits.Models;
using Droits.Models.DTOs.Imports;
using Droits.Models.DTOs.Powerapps;
using Droits.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;

namespace Droits.Controllers;
public class MigrationController : BaseController
{
    private readonly ILogger<MigrationController> _logger;

    private readonly IMigrationService _service;
    private readonly IConfiguration _configuration;
    private const bool DisablePowerappsMigrationEndpoints = true;

    public MigrationController(ILogger<MigrationController> logger, IMigrationService migrationService, IConfiguration configuration)
    {
        _logger = logger;
        _service = migrationService;
        _configuration = configuration;

    }

    public IActionResult Index()
    {
        return RedirectToAction("Index", "Home");
    }
    
    
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> MigrateWreck([FromBody] PowerappsWreckDto request, [FromHeader(Name = "X-API-Key")] string apiKey)
    {

        if (!RequestHelper.IsValidApiKey(apiKey, _configuration))
        {
            return Unauthorized("Invalid API key");
        }

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
    public async Task<IActionResult> MigrateNote([FromBody] PowerappsNoteDto request, [FromHeader(Name = "X-API-Key")] string apiKey)
    {
        if (!RequestHelper.IsValidApiKey(apiKey, _configuration))
        {
            return Unauthorized("Invalid API key");
        }

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
    public async Task<IActionResult> MigrateDroit([FromBody] PowerappsDroitReportDto request, [FromHeader(Name = "X-API-Key")] string apiKey)
    {

        if (!RequestHelper.IsValidApiKey(apiKey, _configuration))
        {
            return Unauthorized("Invalid API key");
        }

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
        [FromBody] PowerappsWreckMaterialDto request, [FromHeader(Name = "X-API-Key")] string apiKey)
    {
        
        if (!RequestHelper.IsValidApiKey(apiKey, _configuration))
        {
            return Unauthorized("Invalid API key");
        }

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
    
    [HttpPost]
    [RequestTimeout(600000)]
    public async Task<IActionResult> ProcessTriageFile(IFormFile? file)
    {
        if ( file == null || file.Length == 0 )
        {
            ModelState.AddModelError("File", "Please select a file");
            return RedirectToAction("UploadTriageFile");
        }
        
        try
        {
            var reader = new StreamReader(file.OpenReadStream());
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<TriageRowDto>().ToList();

            var result = await _service.HandleTriageCsvAsync(records);
            Console.Write("records uploaded");
            
            return View("UploadTriageFile", result);
        }
        catch ( Exception e )
        { 
            HandleError(_logger, "Error updating triage numbers", e);

            return View("UploadTriageFile", new TriageUploadResultDto());
        }

        
    }


    [HttpGet]
    public IActionResult UploadTriageFile()
    {
        return View(new TriageUploadResultDto());
    }
    
    
    [HttpPost]
    public async Task<IActionResult> ProcessAccessFile(IFormFile? file)
    {
        if ( file == null || file.Length == 0 )
        {
            ModelState.AddModelError("File", "Please select a file");
            return RedirectToAction("UploadAccessFile");
        }
        
        try
        {
            var reader = new StreamReader(file.OpenReadStream());
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<AccessDto>().ToList();

            var result = await _service.HandleAccessCsvAsync(records);
            
            Console.Write($"records {result} uploaded");
            
            return View("UploadAccessFile", result);
        }
        catch ( Exception e )
        { 
            HandleError(_logger, "Error uploading Access File", e);

            return View("UploadAccessFile",new AccessUploadResultDto());
        }

        
    }
    
    [HttpGet]
    public IActionResult UploadAccessFile()
    {
        return View(new AccessUploadResultDto());
    }
}