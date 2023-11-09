using Droits.Exceptions;
using Droits.Models.DTOs;
using Droits.Models.Entities;
using Newtonsoft.Json;

namespace Droits.Services;

public interface IApiService
{

    Task<Droit> SaveDroitReportAsync(SubmittedReportDto report);
}

public class ApiService : IApiService
{
    private readonly ILogger<ApiService> _logger;
    private readonly IDroitService _droitService;
    private readonly IWreckMaterialService _wreckMaterialService;
    private readonly ISalvorService _salvorService;
    private readonly ILetterService _letterService;

    
    public ApiService(ILogger<ApiService> logger,  IDroitService droitService, IWreckMaterialService wreckMaterialService, ISalvorService salvorService, ILetterService letterService)
    {
        _logger = logger;
        _droitService = droitService;
        _wreckMaterialService = wreckMaterialService;
        _salvorService = salvorService;
        _letterService = letterService;
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
        var salvor = await _salvorService.GetOrCreateAsync(report);
        
        var droit = await _droitService.CreateDroitAsync(report, salvor);

        if ( droit.Id != default )
        {
            await _wreckMaterialService.CreateWreckMaterialsAsync(report, droit.Id);
        }

        return droit;
    }
}