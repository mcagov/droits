using Droits.Services;
using Microsoft.AspNetCore.Mvc;

namespace Droits.Controllers;

public class ImageController : BaseController
{
    private readonly IImageService _service;
    private readonly IHttpClientFactory _httpClientFactory;

    public ImageController(IImageService service, IHttpClientFactory httpClientFactory)
    {
        _service = service;
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<IActionResult> DisplayImage(Guid id)
    {
        var image = await _service.GetImageAsync(id);
        var stream = await _service.GetImageStreamAsync(image.Key);
        return File(stream, image.FileContentType);
    }
}