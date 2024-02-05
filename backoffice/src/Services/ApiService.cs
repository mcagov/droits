using AutoMapper;
using Droits.Exceptions;
using Droits.Helpers.SearchHelpers;
using Droits.Models.DTOs;
using Droits.Models.DTOs.Powerapps;
using Droits.Models.DTOs.Webapp;
using Droits.Models.Entities;


namespace Droits.Services;

public interface IApiService
{

    Task<Droit> SaveDroitReportAsync(SubmittedReportDto report);
    Task<List<Droit>> MigrateDroitsAsync(PowerappsDroitReportsDto request);
    Task<Droit> MigrateDroitAsync(PowerappsDroitReportDto request);

    Task<WreckMaterial> MigrateWreckMaterialAsync(PowerappsWreckMaterialDto wmRequest);
    Task<List<Wreck>> MigrateWrecksAsync(PowerappsWrecksDto request);
    Task<SalvorInfoDto> GetSalvorInfoAsync(string salvorEmail);
    Task<SalvorInfoReportDto> GetReportByIdAsync(Guid droitId);
    Task<WreckMaterial> GetWreckMaterialAsync(Guid wmId);
    Task<Wreck> MigrateWreckAsync(PowerappsWreckDto request);
    Task<Note> MigrateNoteAsync(PowerappsNoteDto request);


}

public class ApiService : IApiService
{
    private readonly ILogger<ApiService> _logger;
    private readonly IDroitService _droitService;
    private readonly IWreckMaterialService _wreckMaterialService;
    private readonly ISalvorService _salvorService;
    private readonly IImageService _imageService;
    private readonly IWreckService _wreckService;
    private readonly IDroitFileService _fileService;
    private readonly INoteService _noteService;
    
    private readonly IMapper _mapper;


    
    public ApiService(ILogger<ApiService> logger,  IDroitService droitService, IWreckMaterialService wreckMaterialService, ISalvorService salvorService, IImageService imageService, IDroitFileService fileService, IWreckService wreckService, INoteService noteService, IMapper mapper)
    {
        _logger = logger;
        _droitService = droitService;
        _wreckMaterialService = wreckMaterialService;
        _salvorService = salvorService;
        _imageService = imageService;
        _fileService = fileService;
        _wreckService = wreckService;
        _noteService = noteService;
        _mapper = mapper;
    }


    public async Task<Droit> SaveDroitReportAsync(SubmittedReportDto report)
    {

        if ( report == null )
        {
            _logger.LogError("Report is null");
            throw new DroitNotFoundException();
        }
        
        var droit = await MapSubmittedDataAsync(report);

        // Send submission confirmed email 
        //Turned off sending submission emails for now.
        // await _letterService.SendSubmissionConfirmationEmailAsync(droit, report);
        
        return droit;
    }


    public async Task<List<Wreck>> MigrateWrecksAsync(PowerappsWrecksDto wrecksRequest)
    {
        if ( wrecksRequest == null )
        {
            _logger.LogError("Request is null");
            throw new WreckNotFoundException();
        }

        var wrecks = new List<Wreck>();

        if ( wrecksRequest.Value == null ) return wrecks;
        
        foreach ( var powerappsWreckDto in wrecksRequest.Value )
        {

            try
            {
              
                var wreck = await MigrateWreckAsync(powerappsWreckDto);
                wrecks.Add(wreck);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to save wreck - {powerappsWreckDto.Mcawrecksid} {powerappsWreckDto.Name} - {ex}");
            }
            
        }

        return wrecks;
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
            wreck = await _wreckService.SaveWreckAsync(wreck);
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
            note = await _noteService.SaveNoteAsync(note);
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

    
    public async Task<List<Droit>> MigrateDroitsAsync(PowerappsDroitReportsDto droitsRequest)
    {
        if ( droitsRequest == null )
        {
            _logger.LogError("Request is null");
            throw new DroitNotFoundException();
        }

        var droits = new List<Droit>();

        if ( droitsRequest.Value == null ) return droits;
        
        foreach ( var powerappsDroitReportDto in droitsRequest.Value )
        {
            try
            {
                if ( string.IsNullOrEmpty(powerappsDroitReportDto.ReportReference) )
                {
                    continue;
                }
                
                var droit = await MigrateDroitAsync(powerappsDroitReportDto);

                droits.Add(droit);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to save droit - {powerappsDroitReportDto.WreckValue} {powerappsDroitReportDto.ReportReference} - {ex}");
            }
            
        }

        return droits;
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
            
            if ( droitRequest.Reporter != null )
            {
                var mappedSalvor = _mapper.Map<Salvor>(droitRequest.Reporter);
                var salvor = await _salvorService.GetOrCreateAsync(mappedSalvor);

                droit.SalvorId = salvor.Id;
            }
                
            droit = await _droitService.SaveDroitAsync(droit);
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
    
    private async Task<Droit> MapSubmittedDataAsync(SubmittedReportDto report)
    {
        var mappedSalvor = _mapper.Map<Salvor>(report);
        var salvor = await _salvorService.GetOrCreateAsync(mappedSalvor);
        
        var droit = await _droitService.CreateDroitAsync(report, salvor);

        if ( droit.Id != default )
        {
            await _wreckMaterialService.CreateWreckMaterialsAsync(report, droit.Id);
        }

        return droit;
    }


    public async Task<SalvorInfoDto> GetSalvorInfoAsync(string salvorEmail)
    {
        var salvors = await _salvorService.GetSalvorsAsync();

        var salvor = salvors.First();
        try
        {
            var foundSalvor = await _salvorService.GetSalvorByEmailAsync(salvor.Email);
            var salvorInfo = _mapper.Map<SalvorInfoDto>(foundSalvor);

            // var reports = new List<SalvorInfoReportDto>();
            //
            // foreach ( var droit in salvor.Droits )
            // {
            //     var salvorInfoDroit = _mapper.Map<SalvorInfoReportDto>(droit);
            //     reports.Add(salvorInfoDroit);
            // }
            //
            // salvorInfo.Reports = reports.ToArray();

            return salvorInfo;

        }
        catch ( Exception ex )
        {
            _logger.LogError($"Salvor not found.. {ex}" );
            throw new SalvorNotFoundException("Salvor not found");
        }
        
    }


    public async Task<SalvorInfoReportDto> GetReportByIdAsync(Guid id)
    {
        var droit = await _droitService.GetDroitAsync(id);
        var report = _mapper.Map<SalvorInfoReportDto>(droit);

        return report;
    }


    public async Task<WreckMaterial> GetWreckMaterialAsync(Guid wmId)
    {
        var wm = await _wreckMaterialService.GetWreckMaterialAsync(wmId);

        return wm;
    }
}