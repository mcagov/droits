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
    Task<SalvorInfoDto> GetSalvorInfoAsync(string salvorEmail);
    Task<SalvorInfoReportDto> GetReportByIdAsync(Guid droitId);
}

public class ApiService : IApiService
{
    private readonly ILogger<ApiService> _logger;
    private readonly IDroitService _droitService;
    private readonly IWreckMaterialService _wreckMaterialService;
    private readonly ISalvorService _salvorService;
    private readonly ILetterService _letterService;

    private readonly IMapper _mapper;


    
    public ApiService(ILogger<ApiService> logger,  IDroitService droitService, IWreckMaterialService wreckMaterialService, ISalvorService salvorService, ILetterService letterService, IMapper mapper)
    {
        _logger = logger;
        _droitService = droitService;
        _wreckMaterialService = wreckMaterialService;
        _salvorService = salvorService;
        _letterService = letterService;
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
        try
        {
            var foundSalvor = await _salvorService.GetSalvorByEmailAsync(salvorEmail);
            var salvorInfo = _mapper.Map<SalvorInfoDto>(foundSalvor);

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