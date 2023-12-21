#region

using Droits.Clients;
using Droits.Data;
using Droits.Exceptions;
using Droits.Helpers;
using Droits.Models.Entities;
using Droits.Services;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Droits.Repositories;

public interface IDroitFileRepository
{
    Task<DroitFile> AddAsync(DroitFile droitFile);
    Task<DroitFile> UpdateAsync(DroitFile droitFile);
    Task<DroitFile> GetDroitFileAsync(Guid id);
    Task UploadDroitFileAsync(DroitFile droitFile, IFormFile droitFileUpload);
    Task<Stream> GetDroitFileStreamAsync(string? key);
    Task DeleteDroitFilesForWreckMaterialAsync(Guid wmId, IEnumerable<Guid> droitFilesToKeep);
}

public class DroitFileRepository : BaseEntityRepository<DroitFile>, IDroitFileRepository
{
    private readonly ILogger<DroitFileRepository> _logger;
    private readonly ICloudStorageClient _storageClient; 
    public DroitFileRepository(DroitsContext dbContext, ILogger<DroitFileRepository> logger, IAccountService accountService, ICloudStorageClient storageClient) : base(dbContext,accountService)
    {
        _logger = logger;
        _storageClient = storageClient;
    }
    
    public async Task<DroitFile> GetDroitFileAsync(Guid id)
    {
        var droitFile = await Context.DroitFiles
            .FirstOrDefaultAsync(i => i.Id == id);

        if ( droitFile == null )
        {
            throw new FileNotFoundException();
        }

        return droitFile;
    }


    public async Task UploadDroitFileAsync(DroitFile droitFile, IFormFile droitFileUpload)
    {

        if ( droitFile == null )
        {
            throw new FileNotFoundException();
        }
        
        
        if ( droitFile.WreckMaterial == null )
        {
            throw new WreckMaterialNotFoundException();
        }
        
        
        var key = $"Droits/{droitFile.WreckMaterial.DroitId}/WreckMaterials/{droitFile.WreckMaterialId}/DroitFiles/{droitFile.Id}_{droitFileUpload.FileName}";
        
        try
        {
            var contentType = FileHelper.GetContentType(droitFileUpload.FileName);
            await using var stream = droitFileUpload.OpenReadStream();
            await _storageClient.UploadFileAsync(key,stream,contentType);
            droitFile.Filename = droitFileUpload.FileName;
            droitFile.FileContentType = contentType;
            droitFile.Key = key;
            await UpdateAsync(droitFile);
        }
        catch ( Exception e )
        {
            _logger.LogError($"DroitFile {droitFile.Id} could not be saved - {e.Message} ");
        }
    }


    public async Task<Stream> GetDroitFileStreamAsync(string? key) => await _storageClient.GetFileAsync(key);
    
    public async Task DeleteDroitFilesForWreckMaterialAsync(Guid wmId, IEnumerable<Guid> droitFilesToKeep)
    {
        var droitFilesToDelete = await GetDroitFilesToDeleteAsync(wmId, droitFilesToKeep);
    
        await RemoveDroitFilesFromDatabaseAsync(droitFilesToDelete);
    
        await DeleteDroitFilesFromStorageAsync(droitFilesToDelete);
    }

    private async Task<List<DroitFile>> GetDroitFilesToDeleteAsync(Guid wmId, IEnumerable<Guid> droitFilesToKeep)
    {
        return await Context.DroitFiles
            .Where(droitFile => droitFile.WreckMaterialId == wmId && !droitFilesToKeep.Contains(droitFile.Id))
            .ToListAsync();
    }

    private async Task RemoveDroitFilesFromDatabaseAsync(IEnumerable<DroitFile> droitFilesToDelete)
    {
        Context.DroitFiles.RemoveRange(droitFilesToDelete);
        await Context.SaveChangesAsync();
    }

    private async Task DeleteDroitFilesFromStorageAsync(IEnumerable<DroitFile> droitFilesToDelete)
    {
        var deletionTasks = droitFilesToDelete.Where(droitFile =>  !string.IsNullOrEmpty(droitFile.Key)).Select(droitFile => _storageClient.DeleteFileAsync(droitFile.Key));
        await Task.WhenAll(deletionTasks);
    }
}