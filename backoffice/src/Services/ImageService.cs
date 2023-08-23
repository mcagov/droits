using Droits.Exceptions;
using Droits.Repositories;
using Droits.Models.Entities;
using Droits.Models.FormModels;

namespace Droits.Services;

public interface IImageService
{
    Task<Image> GetImageAsync(Guid id);
    Task<Image> SaveImageAsync(Image image);
    Task<Image> SaveImageFormAsync(ImageForm imageForm);
    Task<Stream> GetImageStreamAsync(string key);
    Task DeleteImagesForWreckMaterialAsync(Guid wmId, IEnumerable<Guid> imagesToKeep);
}

public class ImageService : IImageService
{
    private readonly IImageRepository _repo;


    public ImageService(IImageRepository repo)
    {
        _repo = repo;
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


    public async Task<Stream> GetImageStreamAsync(string key) =>  await _repo.GetImageStreamAsync(key);
    public async Task DeleteImagesForWreckMaterialAsync(Guid wmId, IEnumerable<Guid> imagesToKeep)
    {
        await _repo.DeleteImagesForWreckMaterialAsync(wmId, imagesToKeep);
    }
}