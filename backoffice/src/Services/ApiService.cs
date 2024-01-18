using AutoMapper;
using Droits.Exceptions;
using Droits.Helpers.SearchHelpers;
using Droits.Models.DTOs;
using Droits.Models.DTOs.Powerapps;
using Droits.Models.DTOs.Webapp;
using Droits.Models.Entities;
using Droits.Models.Enums;

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
            var wreck = _mapper.Map<Wreck>(powerappsWreckDto);

            try
            {
                if ( string.IsNullOrEmpty(wreck.Name) )
                {
                    continue;
                }
                wreck = await _wreckService.SaveWreckAsync(wreck);

                wrecks.Add(wreck);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to save wreck - {wreck.PowerappsWreckId} {wreck.Name} - {ex}");
            }
            
        }

        return wrecks;
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
}