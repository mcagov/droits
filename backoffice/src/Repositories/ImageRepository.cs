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
    Task ImagesToDeleteForWreckMaterialAsync(Guid wmId, IEnumerable<Guid> imagesToKeep);
}

public class ImageRepository : BaseEntityRepository<Image>, IImageRepository
{
    private readonly IS3Client _client; 
    public ImageRepository(DroitsContext dbContext, IAccountService accountService, IS3Client client) : base(dbContext,accountService)
    {
        _client = client;
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
        var key = $"Droits/{image.WreckMaterial.DroitId}/WreckMaterials/{image.WreckMaterialId}/Images/{image.Id}_{imageFile.FileName}";
      
        try
        {
            await using var stream = imageFile.OpenReadStream();
            await _client.UploadImageAsync(key,stream);
            image.Filename = imageFile.FileName;
            image.FileContentType = imageFile.ContentType;
            image.Key = key;
            await UpdateAsync(image);
        }
        catch ( Exception e )
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public async Task<Stream> GetImageStreamAsync(string key) => await _client.GetImageAsync(key);
    
    public async Task ImagesToDeleteForWreckMaterialAsync(Guid wmId, IEnumerable<Guid> imagesToKeep)
    {
        var images = await Context.Images
            .Where(image => image.WreckMaterialId == wmId && !imagesToKeep.Contains(image.Id))
            .ToListAsync();
        
        //Remove images from s3 bucket
        foreach ( var image in images )
        {
            await _client.DeleteImageAsync(image.Key);
        }

        Context.Images.RemoveRange(images);
        await Context.SaveChangesAsync();
    }
}