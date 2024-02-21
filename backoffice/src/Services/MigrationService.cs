using AutoMapper;
using Droits.Exceptions;
using Droits.Models.DTOs.Powerapps;
using Droits.Models.Entities;


namespace Droits.Services;

public interface IMigrationService
{
    Task<Droit> MigrateDroitAsync(PowerappsDroitReportDto request);
    Task<WreckMaterial> MigrateWreckMaterialAsync(PowerappsWreckMaterialDto wmRequest);
    Task<Wreck> MigrateWreckAsync(PowerappsWreckDto request);
    Task<Note> MigrateNoteAsync(PowerappsNoteDto request);


}

public class MigrationService : IMigrationService
{
    private readonly ILogger<MigrationService> _logger;
    private readonly IDroitService _droitService;
    private readonly IWreckMaterialService _wreckMaterialService;
    private readonly ISalvorService _salvorService;
    private readonly IImageService _imageService;
    private readonly IWreckService _wreckService;
    private readonly IDroitFileService _fileService;
    private readonly INoteService _noteService;
    private readonly IUserService _userService;

    
    private readonly IMapper _mapper;


    
    public MigrationService(ILogger<MigrationService> logger,  IDroitService droitService, IWreckMaterialService wreckMaterialService, ISalvorService salvorService, IImageService imageService, IDroitFileService fileService, IWreckService wreckService, INoteService noteService, IUserService userService, IMapper mapper)
    {
        _logger = logger;
        _droitService = droitService;
        _wreckMaterialService = wreckMaterialService;
        _salvorService = salvorService;
        _imageService = imageService;
        _fileService = fileService;
        _wreckService = wreckService;
        _noteService = noteService;
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<Wreck> MigrateWreckAsync(PowerappsWreckDto wreckRequest)
    {
        if ( wreckRequest == null )
        {
            _logger.LogError("Request is null");
            throw new WreckNotFoundException();
        }


        var wreck = _mapper.Map<Wreck>(wreckRequest);

        try
        {
            if ( string.IsNullOrEmpty(wreck.Name) )
            {
                _logger.LogError("Name is null");
                throw new WreckNotFoundException();
            }
            
            if ( wreckRequest.ModifiedBy != null )
            {
                var mappedUser = _mapper.Map<ApplicationUser>(wreckRequest.ModifiedBy);
                
                
                var modifiedBy = await _userService.GetOrCreateByEmailAddressAsync(mappedUser);

                wreck.LastModifiedByUserId = modifiedBy.Id;
            }

            
            wreck = await _wreckService.AddWreckAsync(wreck, false);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unable to save wreck - {wreck.PowerappsWreckId} {wreck.Name} - {ex}");
        }
            

        return wreck;
    }

    public async Task<Note> MigrateNoteAsync(PowerappsNoteDto noteRequest)
    {
        if ( noteRequest == null )
        {
            _logger.LogError("Request is null");
            throw new WreckNotFoundException();
        }
        
        var note = _mapper.Map<Note>(noteRequest);
        
        if ( string.IsNullOrEmpty(noteRequest.LinkedEntityPowerappsId) )
        {
            throw new Exception(
                $"No linked entity id for note - {noteRequest.PowerappsAnnotationId}");
        }
        switch (noteRequest.LinkedEntityType)
        {
            case "crf99_mcawreckreport":
                note.DroitId = (await _droitService.GetDroitByPowerappsIdAsync(noteRequest.LinkedEntityPowerappsId)).Id;
                break;

            case "crf99_mcawreckmaterial":
                var wreckMaterial =
                    await _wreckMaterialService.GetWreckMaterialByPowerappsIdAsync(noteRequest
                        .LinkedEntityPowerappsId);

                note.Text = $"{note.Text} - For Wreck Material {wreckMaterial.Name}";
                
                note.DroitId = wreckMaterial.DroitId;
                break;

            case "crf99_mcawrecks":
                note.WreckId = (await _wreckService.GetWreckByPowerappsIdAsync(noteRequest.LinkedEntityPowerappsId)).Id;
                break;

            case "contact":
                note.SalvorId = (await _salvorService.GetSalvorByPowerappsIdAsync(noteRequest.LinkedEntityPowerappsId)).Id;
                break;

            default:
                throw new Exception($"Note for type {noteRequest.LinkedEntityType} found - {noteRequest.PowerappsAnnotationId}");
        }
        
        
        
        try
        {
            if ( noteRequest.ModifiedBy != null )
            {
                var mappedUser = _mapper.Map<ApplicationUser>(noteRequest.ModifiedBy);
                
                
                var modifiedBy = await _userService.GetOrCreateByEmailAddressAsync(mappedUser);

                note.LastModifiedByUserId = modifiedBy.Id;
            }
            
            note = await _noteService.AddNoteAsync(note, false);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unable to save note - {noteRequest.PowerappsAnnotationId} {note.Title} - {ex}");
        }
            

        // Add file to note.. 
        
        if (!string.IsNullOrEmpty(noteRequest.DocumentBody) )
        {
            var file = await _fileService.AddFileToNoteAsync(note, noteRequest);
        }
        
        return note;
    }
    
    
    public async Task<Droit> MigrateDroitAsync(PowerappsDroitReportDto droitRequest)
    {
        if ( droitRequest == null )
        {
            _logger.LogError("Request is null");
            throw new DroitNotFoundException();
        }
        
        var droit = _mapper.Map<Droit>(droitRequest);
        
        try
        {
            if ( string.IsNullOrEmpty(droit.Reference) )
            {
                _logger.LogError("Reference is null");
                throw new DroitNotFoundException();
            }

            if ( !string.IsNullOrEmpty(droit.PowerappsWreckId) )
            {
                try
                {
                    var wreck =
                        await _wreckService.GetWreckByPowerappsIdAsync(droit.PowerappsWreckId);
                    droit.WreckId = wreck.Id;
                }
                catch ( WreckNotFoundException ex )
                {
                    _logger.LogError($"Unable to find Wreck by Powerapps ID - {droit.PowerappsWreckId} - {ex}");

                }
            }
            
            
            if ( droitRequest.ModifiedBy != null )
            {
                var mappedUser = _mapper.Map<ApplicationUser>(droitRequest.ModifiedBy);
                
                
                var modifiedBy = await _userService.GetOrCreateByEmailAddressAsync(mappedUser);

                droit.LastModifiedByUserId = modifiedBy.Id;
            }

            
            if ( droitRequest.Receiver != null )
            {
                var mappedUser = _mapper.Map<ApplicationUser>(droitRequest.Receiver);
                
                
                var reporter = await _userService.GetOrCreateByEmailAddressAsync(mappedUser);

                droit.AssignedToUserId = reporter.Id;
            }
            
            if ( droitRequest.Reporter != null )
            {
                var mappedSalvor = _mapper.Map<Salvor>(droitRequest.Reporter);
                var salvor = await _salvorService.GetOrCreateAsync(mappedSalvor);
                
                // Could set the last modified of the salvor to be the same as the last modified of the droit here.. 
                
                droit.SalvorId = salvor.Id;
            }

            droit =  await _droitService.AddDroitAsync(droit, false);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unable to save droit - {droit.PowerappsWreckId} {droit.Reference} - {ex}");
        }
            

        return droit;
    }

    
        
    public async Task<WreckMaterial> MigrateWreckMaterialAsync(PowerappsWreckMaterialDto wmRequest)
    {
        if ( wmRequest == null )
        {
            _logger.LogError("Request is null");
            throw new WreckMaterialNotFoundException();
        }
        
        var wreckMaterial = _mapper.Map<WreckMaterial>(wmRequest);

        try
        {
            if ( string.IsNullOrEmpty(wreckMaterial.Name) )
            {
                _logger.LogError("Name is null");
                throw new WreckMaterialNotFoundException();
            }
            
            if ( wmRequest.ModifiedBy != null )
            {
                var mappedUser = _mapper.Map<ApplicationUser>(wmRequest.ModifiedBy);
                
                
                var modifiedBy = await _userService.GetOrCreateByEmailAddressAsync(mappedUser);

                wreckMaterial.LastModifiedByUserId = modifiedBy.Id;
            }

            // Connect the droit.. 
            Droit droit = null!;
            if ( !string.IsNullOrEmpty(wmRequest.PowerappsDroitId) )
            {
                try
                {
                    droit =
                        await _droitService.GetDroitByPowerappsIdAsync(wmRequest.PowerappsDroitId);
                    wreckMaterial.DroitId = droit.Id;
     
                }
                catch ( DroitNotFoundException ex )
                {
                    _logger.LogError($"Unable to find Droit by Powerapps ID - {wmRequest.PowerappsDroitId} - {ex}");

                }
            }
            
            //save
            wreckMaterial = await _wreckMaterialService.AddWreckMaterialAsync(wreckMaterial);

            if (!string.IsNullOrEmpty(wmRequest.ImageUrl) )
            {
                if (wmRequest.ImageUrl.ToLower().Contains(".blob.core.windows.net/report-uploads"))
                {
                    await _imageService.AddImageByUrlToWreckMaterial(wreckMaterial.Id,
                        wmRequest.ImageUrl);
                }
                else
                {
                    await _fileService.AddFileUrlToWreckMaterial(wreckMaterial.Id, wmRequest.ImageUrl);
                }
               
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unable to save Wreck Material - {wmRequest.PowerappsWreckMaterialId} {wmRequest.Name} - {ex}");
        }
            

        return wreckMaterial;
    }
    
}