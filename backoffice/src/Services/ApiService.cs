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
    private readonly ISalvorService _salvorService;
    private readonly ILetterService _letterService;

    
    public ApiService(ILogger<ApiService> logger,  IDroitService droitService, ISalvorService salvorService, ILetterService letterService)
    {
        _logger = logger;
        _droitService = droitService;
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
        
        // Map submitted data into Droit etc entities

        var droit = await MapSubmittedDataAsync(report);

        // Upload WM images
        // _droitService.SaveWreckMaterialsAsync()
        
        
        //Save entities

        // Send submission confirmed email 
        await _letterService.SendSubmissionConfirmationEmailAsync(droit);
        
        return droit;
    }


    private async Task<Droit> MapSubmittedDataAsync(SubmittedReportDto report)
    {

        
        var salvor = await _salvorService.GetOrCreateAsync(report);

        
        
        var droit = new Droit()
        {
            SalvorId = salvor.Id,
            Salvor = salvor,
            ReportedDate = DateTime.UtcNow,
            OriginalSubmission = JsonConvert.SerializeObject(report),
        };
        
        droit = await _droitService.SaveDroitAsync(droit);

        await _droitService.CreateWreckMaterials(report, droit.Id);

        
        return droit;
    }
}