using Droits.Data;
using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Services;
using Microsoft.EntityFrameworkCore;

namespace Droits.Repositories;

public interface IImageRepository
{
    Task<Image> AddAsync(Image image);
    Task<Image> UpdateAsync(Image image);
    Task<Image> GetImageAsync(Guid Id);
    Image GetRandomImage();
}

public class ImageRepository : BaseEntityRepository<Image>, IImageRepository
{
    public ImageRepository(DroitsContext dbContext, ICurrentUserService currentUserService) : base(dbContext,currentUserService)
    {
        
    }
    
    public async Task<Image> GetImageAsync(Guid Id)
    {
        var image = await Context.Images
            .FirstOrDefaultAsync(i => i.Id == Id);

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
}