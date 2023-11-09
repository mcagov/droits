#region

using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Repositories;

#endregion

namespace Droits.Services;

public interface IWreckMaterialService
{
    Task<WreckMaterial> SaveWreckMaterialAsync(WreckMaterialForm wmForm);
    Task<WreckMaterial>  GetWreckMaterialAsync(Guid id);
    Task DeleteWreckMaterialForDroitAsync(Guid droitId, IEnumerable<Guid> wmToKeep);

}

public class WreckMaterialService : IWreckMaterialService
{

    private readonly IImageService _imageService;
    private readonly ILogger<WreckMaterialService> _logger;
    private readonly IWreckMaterialRepository _repository;


    public WreckMaterialService(IImageService imageService, ILogger<WreckMaterialService> logger, IWreckMaterialRepository repository)
    {
        _imageService = imageService;
        _logger = logger;
        _repository = repository;
    }
    
    public async Task<WreckMaterial> SaveWreckMaterialAsync(WreckMaterialForm wreckMaterialForm)
    {
        WreckMaterial wreckMaterial;
        if ( wreckMaterialForm.Id == default )
        {
            wreckMaterial = await _repository.AddAsync(
                wreckMaterialForm.ApplyChanges(new WreckMaterial()));
        }

        else
        {
            wreckMaterial = await GetWreckMaterialAsync(wreckMaterialForm.Id);
    
            wreckMaterial = wreckMaterialForm.ApplyChanges(wreckMaterial);
    
            wreckMaterial = await UpdateWreckMaterialAsync(wreckMaterial);
        }
        
        await SaveImagesAsync(wreckMaterial.Id, wreckMaterialForm.ImageForms);

        return wreckMaterial;
    }


    public async Task<WreckMaterial> UpdateWreckMaterialAsync(WreckMaterial wreckMaterial)
    {
        return await _repository.UpdateAsync(wreckMaterial);
    }


    public async Task DeleteWreckMaterialForDroitAsync(Guid droitId, IEnumerable<Guid> wmToKeep)
    {
        await _repository.DeleteWreckMaterialForDroitAsync(droitId, wmToKeep);
    }


    public async Task<WreckMaterial> GetWreckMaterialAsync(Guid id)
    {
        return await _repository.GetWreckMaterialAsync(id);
    }
    
     public async Task SaveImagesAsync(Guid wmId,
        List<ImageForm> imageForms)
    {
        var imageIdsToKeep = imageForms.Select(i => i.Id);
        
        await _imageService.DeleteImagesForWreckMaterialAsync(wmId, imageIdsToKeep);
        

        try
        {
            var wreckMaterial = await GetWreckMaterialAsync(wmId);
            
            foreach ( var imageForm in imageForms )
            {
                imageForm.WreckMaterialId = wreckMaterial.Id;

                await _imageService.SaveImageFormAsync(imageForm);
            }
        }
        catch ( WreckMaterialNotFoundException e )
        {
            _logger.LogError("Wreck Material not found", e);
        }
    }
    
}