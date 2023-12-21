
#region

using Droits.Clients;
using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Repositories;

#endregion

namespace Droits.Services;

public interface IImageService
{
    Task<Image> GetImageAsync(Guid id);
    Task<Image> SaveImageAsync(Image image);
    Task<Image> SaveImageFormAsync(ImageForm imageForm);
    Task<Stream> GetImageStreamAsync(string? key);
    Task DeleteImagesForWreckMaterialAsync(Guid wmId, IEnumerable<Guid> imagesToKeep);
    Task AddImageByUrlToWreckMaterial(Guid wreckMaterialId, string? imageUrl);

}

public class ImageService : IImageService
{
    private readonly ILogger<ImageService> _logger;
    private readonly IImageRepository _repo;
    private readonly IAzureBlobClient _azureClient;



    public ImageService(ILogger<ImageService> logger, IImageRepository repo, IAzureBlobClient azureClient)
    {
        _logger = logger;
        _repo = repo;
        _azureClient = azureClient;
    }
    
    public async Task<Image> SaveImageAsync(Image image)
        {
            if (image.Id == default)
            {
                return await AddImageAsync(image);
            }
            return await UpdateImageAsync(image);
        }
    
    private async Task<Image> AddImageAsync(Image image)
    {
        return await _repo.AddAsync(image);
    }

    private async Task<Image> UpdateImageAsync(Image image)
    {
        return await _repo.UpdateAsync(image);
    }
    
    public async Task<Image> GetImageAsync(Guid id) => await _repo.GetImageAsync(id);


    public async Task<Image> SaveImageFormAsync(ImageForm imageForm)
    {

        Image image;

        if ( imageForm.Id == default )
        {
        
            if ( imageForm.ImageFile == null )
            {
                throw new InvalidOperationException();
            }
            
            image = await _repo.AddAsync(imageForm.ApplyChanges(new Image()));
            
            await _repo.UploadImageFileAsync(image, imageForm.ImageFile);

            return image;
        }
        
        
        image =
            await GetImageAsync(imageForm.Id);

        image = imageForm.ApplyChanges(image);

        image = await UpdateImageAsync(image);

        if ( imageForm?.ImageFile != null )
        {
            await _repo.UploadImageFileAsync(image, imageForm.ImageFile);
        }

        return image;
    }


    public async Task<Stream> GetImageStreamAsync(string? key) =>  await _repo.GetImageStreamAsync(key);
    
    
    private async Task<Stream> GetAzureImageStreamAsync(string url) =>  await _azureClient.GetAzureImageStreamAsync(url);

    public async Task DeleteImagesForWreckMaterialAsync(Guid wmId, IEnumerable<Guid> imagesToKeep)
    {
        await _repo.DeleteImagesForWreckMaterialAsync(wmId, imagesToKeep);
    }

    public async Task AddImageByUrlToWreckMaterial(Guid wreckMaterialId, string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            _logger.LogError("Image URL is null or empty.");
            return;
        }

        _logger.LogDebug($"Uploading image with URL: {imageUrl}");

        imageUrl = imageUrl.ToLower();
        try
        {
            Stream imageStream;

            if (imageUrl.Contains(".blob.core.windows.net/report-uploads"))
            {
                imageStream = await GetAzureImageStreamAsync(imageUrl);
            }
            else
            {
                throw new ImageNotFoundException($"Incompatible Image URL {imageUrl}");
            }

            using var ms = new MemoryStream();
            await imageStream.CopyToAsync(ms);
            ms.Position = 0;

            if ( ms is not { Length: > 0 } )
            {
                _logger.LogError("Image stream is null or empty");
                throw new ImageNotFoundException($"Image stream is null or empty {imageUrl}");
            }
            
            
            IFormFile formFile = new FormFile(ms, 0, ms.Length, imageUrl, imageUrl);

            var imageForm = new ImageForm()
            {
                ImageFile = formFile,
                WreckMaterialId = wreckMaterialId
            };

            await SaveImageFormAsync(imageForm);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error uploading image from URL {imageUrl}: {ex.Message}");
            throw new ImageNotFoundException($"Error uploading image from URL {imageUrl}: {ex.Message}");
        }
    }

    
    
}