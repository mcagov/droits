using AutoMapper;
using Droits.Exceptions;
using Droits.Models.DTOs;
using Droits.Models.DTOs.Webapp;
using Droits.Models.Entities;
using Newtonsoft.Json;

namespace Droits.Services;

public interface IApiService
{

    Task<Droit> SaveDroitReportAsync(SubmittedReportDto report);
    Task<WreckMaterial?> SaveWreckMaterialReportAsync(SubmittedWreckMaterialDto wmReport);
    
    Task<SalvorInfoDto> GetSalvorInfoAsync(string salvorEmail);
    Task<SalvorInfoReportDto> GetReportByIdAsync(Guid droitId);
    Task SendConfirmationEmail(Guid droitId);
}

public class ApiService : IApiService
{
    private readonly ILogger<ApiService> _logger;
    private readonly IDroitService _droitService;
    private readonly IWreckMaterialService _wreckMaterialService;
    private readonly ISalvorService _salvorService;
    private readonly IMapper _mapper;
    private readonly ILetterService _letterService;

    
    public ApiService(ILogger<ApiService> logger,  IDroitService droitService, IWreckMaterialService wreckMaterialService, ISalvorService salvorService, ILetterService letterService, IMapper mapper )
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

        return droit;
    }

    
        public async Task SendConfirmationEmail(Guid droitId)
        {
    
            if ( droitId == default )
            {
                throw new DroitNotFoundException("Invalid Droit id for Confirmation email");
            }
            
            var droit = await _droitService.GetDroitWithAssociationsAsync(droitId);
    
            // Send submission confirmed email 
            await _letterService.SendSubmissionConfirmationEmailAsync(droit);
        }

        

    public async Task<WreckMaterial?> SaveWreckMaterialReportAsync(
        SubmittedWreckMaterialDto wmReport)
    {

        if ( wmReport == null )
        {
            throw new WreckMaterialNotFoundException("Wreck Material Submission is Null");
        }
        
        if ( wmReport.DroitId == default )
        {
            throw new DroitNotFoundException("Invalid Droit id for Wreck Material");
        }
        
        var wreckMaterial = await _wreckMaterialService.CreateWreckMaterialAsync(wmReport);

        if ( wreckMaterial == null )
        {
            return null;
            
        }

        if ( wmReport.AppendToOriginalSubmission )
        {
            await AppendWreckMaterialToDroitOriginalSubmissionAsync(wreckMaterial, wmReport);
        }

        return wreckMaterial;
    }


    private async Task AppendWreckMaterialToDroitOriginalSubmissionAsync(
        WreckMaterial wreckMaterial, SubmittedWreckMaterialDto wmReport)
    {
        var droit = await _droitService.GetDroitAsync(wreckMaterial.DroitId);

        var originalSubmission = droit.OriginalSubmission;
        
        if (string.IsNullOrEmpty(originalSubmission))
        {
            return;
        }

        var droitReport = JsonConvert.DeserializeObject<SubmittedReportDto>(originalSubmission);
        
        if ( droitReport == null ) return;

        droitReport.WreckMaterials ??= new List<SubmittedWreckMaterialDto>();
        droitReport.WreckMaterials.Add(wmReport);
        
        droit.OriginalSubmission = JsonConvert.SerializeObject(droitReport);
        await _droitService.SaveDroitAsync(droit);
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

}