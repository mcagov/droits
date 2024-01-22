#region

using Droits.Exceptions;
using Droits.Helpers;
using Droits.Services;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace Droits.Controllers;

public class FileController : BaseController
{
    private readonly IDroitFileService _service;
    private readonly ILogger<ImageController> _logger;
    public FileController(IDroitFileService service, ILogger<ImageController> logger )
    {
        _service = service;
        _logger = logger;

    }
    
    public async Task<IActionResult> Download(Guid id)
    {
        
        var droitFile = await _service.GetDroitFileAsync(id);

        if ( string.IsNullOrEmpty(droitFile.Filename) )
        {
            return NotFound();
        }
        
        var stream = await _service.GetDroitFileStreamAsync(droitFile.Key);
        try
        {
            return File(stream, FileHelper.GetContentType(droitFile.Filename),droitFile.Filename);
        }
        catch ( FileNotFoundException e )
        {
            _logger.LogError(e,"Could not retrieve File");
            return NotFound();
        }
        
    }
    
    public async Task<IActionResult> Preview(Guid id)
    {
        var droitFile = await _service.GetDroitFileAsync(id);

        if (string.IsNullOrEmpty(droitFile.Filename))
        {
            return NotFound();
        }

        var stream = await _service.GetDroitFileStreamAsync(droitFile.Key);

        try
        {
            return File(stream, FileHelper.GetContentType(droitFile.Filename));
        }
        catch (FileNotFoundException e)
        {
            _logger.LogError(e, "Could not retrieve File");
            return NotFound();
        }
    }
}