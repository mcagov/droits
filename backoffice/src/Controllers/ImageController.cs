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
    
    public async Task<IActionResult> DisplayImage(Guid Id)
    {
        var imageUrl = await _service.GetImageUrlAsync(Id);

        using (var httpClient = _httpClientFactory.CreateClient())
        {
            var response = await httpClient.GetAsync(imageUrl);

            if (response.IsSuccessStatusCode)
            {
                var imageStream = await response.Content.ReadAsStreamAsync();
                return File(imageStream, "image/jpeg"); // Adjust content type as needed
            }
            else
            {
                return NotFound(); // Image not found or other error
            }
        }
    }
}