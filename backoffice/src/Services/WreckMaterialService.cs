#region

using AutoMapper;
using Droits.Exceptions;
using Droits.Models.DTOs;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Repositories;

#endregion

namespace Droits.Services;

public interface IWreckMaterialService
{
    Task<WreckMaterial> SaveWreckMaterialAsync(WreckMaterialForm wmForm);
    Task<WreckMaterial> AddWreckMaterialAsync(WreckMaterial wreckMaterial);
    Task<WreckMaterial?> CreateWreckMaterialAsync(SubmittedWreckMaterialDto? wmReport);
    Task<WreckMaterial>  GetWreckMaterialAsync(Guid id);
    Task DeleteWreckMaterialForDroitAsync(Guid droitId, IEnumerable<Guid> wmToKeep);
    Task CreateWreckMaterialsAsync(SubmittedReportDto report, Guid droitId);
    Task<WreckMaterial> GetWreckMaterialByPowerappsIdAsync(string powerappsId);

}

public class WreckMaterialService : IWreckMaterialService
{

    private readonly IImageService _imageService;
    private readonly IDroitFileService _fileService;
    private readonly ILogger<WreckMaterialService> _logger;
    private readonly IWreckMaterialRepository _repository;
    private readonly IMapper _mapper;



    public WreckMaterialService(IImageService imageService, IDroitFileService fileService, ILogger<WreckMaterialService> logger, IWreckMaterialRepository repository, IMapper mapper)
    {
        _imageService = imageService;
        _fileService = fileService;
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<WreckMaterial> SaveWreckMaterialAsync(WreckMaterialForm wreckMaterialForm)
    {
        WreckMaterial wreckMaterial;
        if ( wreckMaterialForm.Id == default )
        {
            wreckMaterial = await AddWreckMaterialAsync(
                wreckMaterialForm.ApplyChanges(new WreckMaterial()));
        }

        else
        {
            wreckMaterial = await GetWreckMaterialAsync(wreckMaterialForm.Id);
    
            wreckMaterial = wreckMaterialForm.ApplyChanges(wreckMaterial);
    
            wreckMaterial = await UpdateWreckMaterialAsync(wreckMaterial);
        }
        
        await SaveImagesAsync(wreckMaterial.Id, wreckMaterialForm.ImageForms);
        await SaveFilesAsync(wreckMaterial.Id, wreckMaterialForm.DroitFileForms);
        
        return wreckMaterial;
    }


    public async Task<WreckMaterial> AddWreckMaterialAsync(WreckMaterial wreckMaterial) => 
        await _repository.AddAsync(wreckMaterial);



    private async Task<WreckMaterial> UpdateWreckMaterialAsync(WreckMaterial wreckMaterial)
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
    
     private async Task SaveImagesAsync(Guid wmId,
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
            _logger.LogError($"Wreck Material not found - {e}");
        }
    }
     
     private async Task SaveFilesAsync(Guid wmId,
         List<DroitFileForm> fileForms)
     {
         var fileIdsToKeep = fileForms.Select(f => f.Id);
        
         await _fileService.DeleteDroitFilesForWreckMaterialAsync(wmId, fileIdsToKeep);
        

         try
         {
             var wreckMaterial = await GetWreckMaterialAsync(wmId);
            
             foreach ( var fileForm in fileForms )
             {
                 fileForm.WreckMaterialId = wreckMaterial.Id;

                 await _fileService.SaveDroitFileFormAsync(fileForm);
             }
            
         }
         catch ( WreckMaterialNotFoundException e )
         {
             _logger.LogError($"Wreck Material not found - {e}");
         }
     }

     public async Task<WreckMaterial?> CreateWreckMaterialAsync(SubmittedWreckMaterialDto? wmReport)
     {
         if ( wmReport == null )
         {
             _logger.LogError("No Wreck Material Submitted");
             return null;
         }

         if ( wmReport.DroitId == null)
         {            
             _logger.LogError("No Droit for Wreck Material Submitted");
             return null;
         }
         
         var wreckMaterial = _mapper.Map<WreckMaterial>(wmReport);
          wreckMaterial.DroitId = wmReport.DroitId.Value;

          wreckMaterial = await AddWreckMaterialAsync(wreckMaterial);

             if ( wmReport.Image != null )
             {
                 await SaveSubmittedImageAsync(wreckMaterial.Id, wmReport.Image);
             }

             return wreckMaterial;
     }
     
     public async Task CreateWreckMaterialsAsync(SubmittedReportDto report, Guid droitId)
     {
         if ( report.WreckMaterials == null || !report.WreckMaterials.Any() )
         {
             _logger.LogError("No Wreck Materials for Submitted report");
             return;
         }

         foreach (var wreckMaterialSubmission in report.WreckMaterials)
         {
             wreckMaterialSubmission.DroitId = droitId;
             await CreateWreckMaterialAsync(wreckMaterialSubmission);
             var wreckMaterial = _mapper.Map<WreckMaterial>(wreckMaterialSubmission);
             wreckMaterial.DroitId = droitId;

             await AddWreckMaterialAsync(wreckMaterial);

             if ( wreckMaterialSubmission.Image != null )
             {
                 await SaveSubmittedImageAsync(wreckMaterial.Id, wreckMaterialSubmission.Image);
             }
         }
     }


     public async Task<WreckMaterial> GetWreckMaterialByPowerappsIdAsync(string powerappsId) =>
         await _repository.GetWreckMaterialByPowerappsIdAsync(powerappsId);


     private async Task SaveSubmittedImageAsync(Guid wreckMaterialId, SubmittedImageDto image)
     {
         if ( string.IsNullOrEmpty(image.Data) || string.IsNullOrEmpty(image.Filename) )
         {
             throw new Exception("Unable to create image without data or filename");
         }
         
         var fileBytes = Convert.FromBase64String(image.Data);
         using var ms = new MemoryStream(fileBytes);
         IFormFile formFile = new FormFile(ms, 0, ms.Length, image.Filename, image.Filename);
         
         var imageForm = new ImageForm()
         {
             ImageFile = formFile,
             WreckMaterialId = wreckMaterialId
         };

         await _imageService.SaveImageFormAsync(imageForm);
     }
     
     
}