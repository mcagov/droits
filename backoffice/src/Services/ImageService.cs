using Droits.Repositories;
using Droits.Models.Entities;
using Droits.Models.FormModels;

namespace Droits.Services;

public interface IImageService
{
    Task<string> GetImageUrlAsync(Guid Id);
    string GetRandomImageUrl();
    Task<Image> SaveImageAsync(Image image);
    Task<Image> SaveImageFormAsync(ImageForm imageForm);
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

    public async Task<Image> GetImageAsync(Guid id)
    {
        return await _repo.GetImageAsync(id);
    }
    
    public string GetRandomImageUrl()
    {
        return _repo.GetRandomImage().Url;
    }
    public async Task<string> GetImageUrlAsync(Guid id)
    {
        var image = await _repo.GetImageAsync(id);
        return image.Url;
    }


    public async Task<Image> SaveImageFormAsync(ImageForm imageForm)
    {
        if ( imageForm.Id == default )
        {
            return await _repo.AddAsync(
                imageForm.ApplyChanges(new Image()));
        }

        var image =
            await GetImageAsync(imageForm.Id);

        image = imageForm.ApplyChanges(image);

        image = await UpdateImageAsync(image);
        
        return image;
    }
}