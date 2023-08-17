using Droits.Services;
using Microsoft.AspNetCore.Mvc;

namespace Droits.Controllers;

public class ImageController : BaseController
{
    private readonly IImageService _service;
    private readonly IHttpClientFactory _httpClientFactory;
    
    //Move this to config. 
    private readonly string _imageEndpoint = "https://droits-wreck-images-dev.s3.eu-west-2.amazonaws.com";
    public ImageController(IImageService service, IHttpClientFactory httpClientFactory)
    {
        _service = service;
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<IActionResult> DisplayImage(Guid id)
    {
        var image = await _service.GetImageAsync(id);
        var stream = await _service.GetImageStreamAsync(image.Key);
        return File(stream, image.FileContentType); // Adjust content type as needed
    }
    
    public async Task<IActionResult> DisplayRandomImage()
    {
        var imageUrl = _service.GetRandomImageKey();

        using (var httpClient = _httpClientFactory.CreateClient())
        {
            var response = await httpClient.GetAsync(imageUrl); //change this

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