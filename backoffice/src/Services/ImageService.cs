using Droits.Repositories;

namespace Droits.Services;

public interface IImageService
{
    Task<string> GetImageUrlAsync(Guid Id);
}

public class ImageService : IImageService
{
    private readonly IImageRepository _repo;


    public ImageService(IImageRepository repo)
    {
        _repo = repo;
    }
    public async Task<string> GetImageUrlAsync(Guid Id)
    {
        var image = await _repo.GetImageAsync(Id);
        return image.Url;
    }
}