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
    Image GetRandomImage();
    Task UploadImageFileAsync(Guid id, IFormFile imageFile);
    Task<Stream> GetImageStreamAsync(string key);
}

public class ImageRepository : BaseEntityRepository<Image>, IImageRepository
{
    private readonly IS3Client _client; 
    public ImageRepository(DroitsContext dbContext, ICurrentUserService currentUserService, IS3Client client) : base(dbContext,currentUserService)
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
    
    public Image GetRandomImage()
    {
        var image = Context.Images
            .First();

        if ( image == null )
        {
            throw new ImageNotFoundException();
        }

        return image;
    }


    public async Task UploadImageFileAsync(Guid id, IFormFile imageFile)
    {
        var key = $"{id}_{Guid.NewGuid()}.{imageFile.FileName}";
      
        try
        {
            await using var stream = imageFile.OpenReadStream();
            await _client.UploadImageAsync(key,stream);
            var image = await GetImageAsync(id);
            image.Filename = imageFile.FileName;
            image.FileContentType = imageFile.ContentType;
            image.Key =  key;
            await UpdateAsync(image);
        }
        catch ( Exception e )
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public async Task<Stream> GetImageStreamAsync(string key) => await _client.GetImageAsync(key);

}