using System.Security.AccessControl;
using Droits.Clients;
using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Repositories;

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
        if ( wreckMaterialForm.Id == default )
        {
            return await _repository.AddAsync(
                wreckMaterialForm.ApplyChanges(new WreckMaterial()));
        }

        var wreckMaterial =
            await GetWreckMaterialAsync(wreckMaterialForm.Id);

        wreckMaterial = wreckMaterialForm.ApplyChanges(wreckMaterial);

        wreckMaterial = await UpdateWreckMaterialAsync(wreckMaterial);

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
        // var wreckMaterialIdsToKeep = imageForms.Select(wm => wm.Id);
        //
        // await _imageService.DeleteWreckMaterialForDroitAsync(droitId, imageIdsToKeep);

        try
        {
            var wreckMaterial = await GetWreckMaterialAsync(wmId);
            
            foreach ( var imageForm in imageForms )
            {
                imageForm.WreckMaterialId = wmId;

                await _imageService.SaveImageFormAsync(imageForm);
            }
        }
        catch ( WreckMaterialNotFoundException e )
        {
            _logger.LogError("Wreck Material not found", e);
        }
    }
    
}