using Droits.Repositories;
using Droits.Models.Entities;
using Droits.Models.FormModels;

namespace Droits.Services;

public interface IImageService
{
    Task<Image> GetImageAsync(Guid id);
    string GetRandomImageKey();
    Task<Image> SaveImageAsync(Image image);
    Task<Image> SaveImageFormAsync(ImageForm imageForm);
    Task<Stream> GetImageStreamAsync(string key);
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
    
    public string GetRandomImageKey()
    {
        return _repo.GetRandomImage().Key;
    }
    public async Task<Image> GetImageAsync(Guid id) => await _repo.GetImageAsync(id);


    public async Task<Image> SaveImageFormAsync(ImageForm imageForm)
    {

        Image image;
        
        if ( imageForm.Id == default )
        {
             
            image = await _repo.AddAsync(
                imageForm.ApplyChanges(new Image()));
            
           await _repo.UploadImageFileAsync(image.Id, imageForm.ImageFile);

            return image;
        }
        
        
        image =
            await GetImageAsync(imageForm.Id);

        image = imageForm.ApplyChanges(image);

        image = await UpdateImageAsync(image);

        await _repo.UploadImageFileAsync(image.Id, imageForm.ImageFile);


        return image;
    }


    public async Task<Stream> GetImageStreamAsync(string key) =>  await _repo.GetImageStreamAsync(key);

}