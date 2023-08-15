using Droits.Repositories;
using Droits.Models.Entities;

namespace Droits.Services;

public interface IImageService
{
    Task<string> GetImageUrlAsync(Guid Id);
    string GetRandomImageUrl();
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
    public async Task<string> GetImageUrlAsync(Guid Id)
    {
        var image = await _repo.GetImageAsync(Id);
        return image.Url;
    }
}