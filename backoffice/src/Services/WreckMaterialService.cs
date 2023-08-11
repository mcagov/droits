using Droits.Clients;
using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Repositories;

namespace Droits.Services;

public interface IWreckMaterialService
{
    Task UploadImageAsync(string key, Stream imageStream);
    Task<Stream> GetImageAsync(string key);
    Task<String> GetImageTypeAsync(string key);
    Task<WreckMaterial> SaveWreckMaterialAsync(WreckMaterialForm wmForm);
    Task<WreckMaterial>  UpdateWreckMaterialAsync(WreckMaterial wm);
    Task DeleteWreckMaterialForDroitAsync(Guid droitId, IEnumerable<Guid> wmToKeep);

}

public class WreckMaterialService : IWreckMaterialService
{
    
    private readonly IS3Client _client;
    private readonly ILogger<WreckMaterialService> _logger;
    private readonly IWreckMaterialRepository _repository;


    public WreckMaterialService(IS3Client client, ILogger<WreckMaterialService> logger, IWreckMaterialRepository repository)
    {
        _client = client;
        _logger = logger;
        _repository = repository;
    }
     
    public async Task UploadImageAsync(string key, Stream imageStream)
    {
        await _client.UploadImageAsync(key, imageStream);
    }


    public async Task<Stream> GetImageAsync(string key)
    {
        return await _client.GetImageAsync(key);
    }


    public async Task<string> GetImageTypeAsync(string key)
    {
        return await _client.GetImageTypeAsync(key);
    }
    
    public async Task<WreckMaterial> SaveWreckMaterialAsync(WreckMaterialForm wreckMaterialForm)
    {
        if ( wreckMaterialForm.Id == default )
        {
            return await _repository.AddAsync(
                wreckMaterialForm.ApplyChanges(new WreckMaterial()));
        }

        var wreckMaterial =
            await _repository.GetWreckMaterialAsync(wreckMaterialForm.Id, wreckMaterialForm.DroitId);

        wreckMaterial = wreckMaterialForm.ApplyChanges(wreckMaterial);

        return await UpdateWreckMaterialAsync(wreckMaterial);
    }


    public async Task<WreckMaterial> UpdateWreckMaterialAsync(WreckMaterial wreckMaterial)
    {
        return await _repository.UpdateAsync(wreckMaterial);
    }


    public async Task DeleteWreckMaterialForDroitAsync(Guid droitId, IEnumerable<Guid> wmToKeep)
    {
        await _repository.DeleteWreckMaterialForDroitAsync(droitId, wmToKeep);
    }
    
}