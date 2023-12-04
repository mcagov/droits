using AutoMapper;
using Droits.Exceptions;
using Droits.Models.DTOs;
using Droits.Models.DTOs.Powerapps;
using Droits.Models.Entities;

namespace Droits.Services;

public interface IApiService
{

    Task<Droit> SaveDroitReportAsync(SubmittedReportDto report);
    Task<List<Wreck>> SaveWrecksAsync(PowerappsWrecksDto request);
}

public class ApiService : IApiService
{
    private readonly ILogger<ApiService> _logger;
    private readonly IDroitService _droitService;
    private readonly IWreckMaterialService _wreckMaterialService;
    private readonly ISalvorService _salvorService;
    private readonly ILetterService _letterService;
    private readonly IWreckService _wreckService;
    
    private readonly IMapper _mapper;


    
    public ApiService(ILogger<ApiService> logger,  IDroitService droitService, IWreckMaterialService wreckMaterialService, ISalvorService salvorService, ILetterService letterService, IWreckService wreckService, IMapper mapper)
    {
        _logger = logger;
        _droitService = droitService;
        _wreckMaterialService = wreckMaterialService;
        _salvorService = salvorService;
        _letterService = letterService;
        _wreckService = wreckService;
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
        await _letterService.SendSubmissionConfirmationEmailAsync(droit, report);
        
        return droit;
    }


    public async Task<List<Wreck>> SaveWrecksAsync(PowerappsWrecksDto wrecksRequest)
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


    private async Task<Droit> MapSubmittedDataAsync(SubmittedReportDto report)
    {
        var salvor = await _salvorService.GetOrCreateAsync(report);
        
        var droit = await _droitService.CreateDroitAsync(report, salvor);

        if ( droit.Id != default )
        {
            await _wreckMaterialService.CreateWreckMaterialsAsync(report, droit.Id);
        }

        return droit;
    }
}