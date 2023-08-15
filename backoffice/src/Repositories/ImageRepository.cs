using Droits.Data;
using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Services;
using Microsoft.EntityFrameworkCore;

namespace Droits.Repositories;

public interface IImageRepository
{
    Task<Image> GetImageAsync(Guid Id);
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
}