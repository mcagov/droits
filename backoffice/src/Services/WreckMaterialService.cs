using Droits.Clients;
using Droits.Models.Entities;

namespace Droits.Services;

public interface IWreckMaterialService
{
    Task UploadImageAsync(string key, Stream imageStream);
    Task<Stream> GetImageAsync(string key);
    Task<String> GetImageTypeAsync(string key);
}

public class WreckMaterialService : IWreckMaterialService
{
    
    private readonly IS3Client _client;
    private readonly ILogger<WreckMaterialService> _logger;


    public WreckMaterialService(IS3Client client, ILogger<WreckMaterialService> logger)
    {
        _client = client;
        _logger = logger;
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
}