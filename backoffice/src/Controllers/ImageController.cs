using Droits.Exceptions;
using Droits.Services;
using Microsoft.AspNetCore.Mvc;

namespace Droits.Controllers;

public class ImageController : BaseController
{
    private readonly IImageService _service;
    private readonly ILogger<ImageController> _logger;

    public ImageController(IImageService service, ILogger<ImageController> logger )
    {
        _service = service;
        _logger = logger;
    }
    
    public async Task<IActionResult> DisplayImage(Guid id)
    {
        var image = await _service.GetImageAsync(id);
        var stream = await _service.GetImageStreamAsync(image.Key);
        try
        {
            return File(stream, image.FileContentType);
        }
        catch ( ImageNotFoundException e )
        {
            _logger.LogError(e,"Could not retrieve Image");
            return NotFound();
        }
        
    }
}