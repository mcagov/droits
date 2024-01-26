#region

using Droits.Helpers;
using Droits.Models.DTOs.Powerapps;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Repositories;

#endregion

namespace Droits.Services;

public interface IDroitFileService
{
    Task<DroitFile> GetDroitFileAsync(Guid id);
    Task<DroitFile> SaveDroitFileAsync(DroitFile droitFile);
    Task<DroitFile> SaveDroitFileFormAsync(DroitFileForm droitFileForm);
    Task<Stream> GetDroitFileStreamAsync(string? key);
    Task DeleteDroitFilesForWreckMaterialAsync(Guid wmId, IEnumerable<Guid> droitFilesToKeep);
    Task DeleteDroitFilesForNoteAsync(Guid noteId, IEnumerable<Guid> droitFilesToKeep);

    Task AddFileUrlToWreckMaterial(Guid wreckMaterialId, string wmRequestImageUrl);
    Task<DroitFile> AddFileToNoteAsync(Note note, PowerappsNoteDto noteRequest);
}

public class DroitFileService : IDroitFileService
{
    private readonly ILogger<DroitFileService> _logger;
    private readonly IDroitFileRepository _repo;


    public DroitFileService(ILogger<DroitFileService> logger, IDroitFileRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }


    public async Task<DroitFile> SaveDroitFileAsync(DroitFile droitFile)
    {
        if ( droitFile.Id == default )
        {
            return await AddDroitFileAsync(droitFile);
        }

        return await UpdateDroitFileAsync(droitFile);
    }


    private async Task<DroitFile> AddDroitFileAsync(DroitFile droitFile)
    {
        return await _repo.AddAsync(droitFile);
    }


    private async Task<DroitFile> UpdateDroitFileAsync(DroitFile droitFile)
    {
        return await _repo.UpdateAsync(droitFile);
    }


    public async Task<DroitFile> GetDroitFileAsync(Guid id) => await _repo.GetDroitFileAsync(id);


    public async Task<DroitFile> SaveDroitFileFormAsync(DroitFileForm droitFileForm)
    {
        DroitFile droitFile;

        if ( droitFileForm.Id == default )
        {
            droitFile = await _repo.AddAsync(droitFileForm.ApplyChanges(new DroitFile()));

            if ( droitFileForm.DroitFile != null )
            {
                await _repo.UploadDroitFileAsync(droitFile, droitFileForm.DroitFile);
            }
            
            return droitFile;
        }


        droitFile =
            await GetDroitFileAsync(droitFileForm.Id);

        droitFile = droitFileForm.ApplyChanges(droitFile);

        droitFile = await UpdateDroitFileAsync(droitFile);

        if ( droitFileForm?.DroitFile != null )
        {
            await _repo.UploadDroitFileAsync(droitFile, droitFileForm.DroitFile);
        }

        return droitFile;
    }


    public async Task<Stream> GetDroitFileStreamAsync(string? key) =>
        await _repo.GetDroitFileStreamAsync(key);
    


    public async Task DeleteDroitFilesForWreckMaterialAsync(Guid wmId,
        IEnumerable<Guid> droitFilesToKeep)
    {
        await _repo.DeleteDroitFilesForWreckMaterialAsync(wmId, droitFilesToKeep);
    }

    public async Task DeleteDroitFilesForNoteAsync(Guid noteId,
        IEnumerable<Guid> droitFilesToKeep)
    {
        await _repo.DeleteDroitFilesForNoteAsync(noteId, droitFilesToKeep);
    }


    public async Task AddFileUrlToWreckMaterial(Guid wreckMaterialId, string wmRequestImageUrl)
    {
        var file = new DroitFile()
        {
            WreckMaterialId = wreckMaterialId,
            Url = wmRequestImageUrl,
            Title = "Imported Url"
        };

        await _repo.AddAsync(file);

    }


    public async Task<DroitFile> AddFileToNoteAsync(Note note, PowerappsNoteDto noteRequest)
    {
        var documentBody = noteRequest.DocumentBody;
        var fileName = noteRequest.FileName;

        if (string.IsNullOrEmpty(documentBody))
        {
            _logger.LogError("Base64-encoded file content is null or empty");
            throw new ArgumentException("Base64-encoded file content is null or empty", nameof(documentBody));
        }

        byte[] fileBytes = Convert.FromBase64String(documentBody);

        using var ms = new MemoryStream(fileBytes);

        if (ms.Length == 0)
        {
            _logger.LogError("File stream is null or empty");
            throw new FileNotFoundException("File stream is null or empty");
        }

        IFormFile formFile = new FormFile(ms, 0, ms.Length, fileName, fileName);
        
        var fileForm = new DroitFileForm()
        {
            NoteId = note.Id,
            DroitFile = formFile,
            Filename = fileName,
            Title = fileName
        };

        var file = await SaveDroitFileFormAsync(fileForm);
        return file;
    }

}