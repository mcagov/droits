#region

using Droits.Exceptions;
using Droits.Helpers;
using Droits.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#endregion

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
    
    [AllowAnonymous]
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
    
    // public async Task<IActionResult> DisplayAzureImage(string url)
    // {
    //     var stream = await _service.GetAzureImageStreamAsync(url);
    //     try
    //     {
    //         return File(stream, FileHelper.GetContentType(url));
    //     }
    //     catch ( ImageNotFoundException e )
    //     {
    //         _logger.LogError(e,"Could not retrieve Image");
    //         return NotFound();
    //     }
    //     
    // }
    
}