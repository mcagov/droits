using Droits.Exceptions;
using Droits.Models;
using Droits.Models.DTOs;
using Droits.Models.Entities;

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


    private readonly Random _random;
    
    public ApiService(ILogger<ApiService> logger,  IDroitService droitService, ISalvorService salvorService, ILetterService letterService)
    {
        _logger = logger;
        _droitService = droitService;
        _salvorService = salvorService;
        _letterService = letterService;

        _random = new Random();
    }


    public async Task<Droit> SaveDroitReportAsync(SubmittedReportDto report)
    {

        if ( report == null )
        {
            _logger.LogError("Report is null");
            throw new DroitNotFoundException();
        }
        
        // Map submitted data into Droit etc entities

        var droit = await MapSubmittedData(report);
        
        
        // Store the original submitted data against the droit
        // droit.OriginalSubmission = xxx;
        
        // Upload WM images
        // _droitService.SaveWreckMaterialsAsync()
        
        
        //Save entities
        
        // Send submission confirmed email 
        // _letterService.SendSubmissionConfirmationEmailAsync(droit);
        return droit;
    }


    private async Task<Droit> MapSubmittedData(SubmittedReportDto report)
    {

        
        var salvor = await _salvorService.GetOrCreateAsync(report);

        // var wreckMaterials = _droitService.CreateWreckMaterials(report);
        
        
        var droit = new Droit()
        {
            SalvorId = salvor.Id,
            ReportedDate = DateTime.UtcNow,
        };


        await _droitService.SaveDroitAsync(droit);
        
        return droit;
    }
}