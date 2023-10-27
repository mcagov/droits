using Droits.Clients;
using Droits.Data;
using Droits.Exceptions;
using Droits.Helpers;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Services;
using Microsoft.EntityFrameworkCore;

namespace Droits.Repositories;

public interface IImageRepository
{
    Task<Image> AddAsync(Image image);
    Task<Image> UpdateAsync(Image image);
    Task<Image> GetImageAsync(Guid id);
    Task UploadImageFileAsync(Image image, IFormFile imageFile);
    Task<Stream> GetImageStreamAsync(string key);
    Task DeleteImagesForWreckMaterialAsync(Guid wmId, IEnumerable<Guid> imagesToKeep);
}

public class ImageRepository : BaseEntityRepository<Image>, IImageRepository
{
    private readonly ILogger<ImageRepository> _logger;
    private readonly IImageStorageClient _storageClient; 
    public ImageRepository(DroitsContext dbContext, ILogger<ImageRepository> logger, IAccountService accountService, IImageStorageClient storageClient) : base(dbContext,accountService)
    {
        _logger = logger;
        _storageClient = storageClient;
    }
    
    public async Task<Image> GetImageAsync(Guid id)
    {
        var image = await Context.Images
            .FirstOrDefaultAsync(i => i.Id == id);

        if ( image == null )
        {
            throw new ImageNotFoundException();
        }

        return image;
    }


    public async Task UploadImageFileAsync(Image image, IFormFile imageFile)
    {

        if ( image == null )
        {
            throw new ImageNotFoundException();
        }
        
        
        if ( image.WreckMaterial == null )
        {
            throw new WreckMaterialNotFoundException();
        }
        
        
        var key = $"Droits/{image.WreckMaterial.DroitId}/WreckMaterials/{image.WreckMaterialId}/Images/{image.Id}_{imageFile.FileName}";
      
        try
        {
            await using var stream = imageFile.OpenReadStream();
            await _storageClient.UploadImageAsync(key,stream,imageFile.ContentType);
            image.Filename = imageFile.FileName;
            image.FileContentType = imageFile.ContentType;
            image.Key = key;
            await UpdateAsync(image);
        }
        catch ( Exception e )
        {
            _logger.LogError($"Image {image.Id} could not be saved - {e.Message} ");
        }
    }


    public async Task<Stream> GetImageStreamAsync(string key) => await _storageClient.GetImageAsync(key);
    
    public async Task DeleteImagesForWreckMaterialAsync(Guid wmId, IEnumerable<Guid> imagesToKeep)
    {
        var imagesToDelete = await GetImagesToDeleteAsync(wmId, imagesToKeep);
    
        await RemoveImagesFromDatabaseAsync(imagesToDelete);
    
        await DeleteImagesFromStorageAsync(imagesToDelete);
    }

    private async Task<List<Image>> GetImagesToDeleteAsync(Guid wmId, IEnumerable<Guid> imagesToKeep)
    {
        return await Context.Images
            .Where(image => image.WreckMaterialId == wmId && !imagesToKeep.Contains(image.Id))
            .ToListAsync();
    }

    private async Task RemoveImagesFromDatabaseAsync(IEnumerable<Image> imagesToDelete)
    {
        Context.Images.RemoveRange(imagesToDelete);
        await Context.SaveChangesAsync();
    }

    private async Task DeleteImagesFromStorageAsync(IEnumerable<Image> imagesToDelete)
    {
        var deletionTasks = imagesToDelete.Select(image => _storageClient.DeleteImageAsync(image.Key));
        await Task.WhenAll(deletionTasks);
    }
}