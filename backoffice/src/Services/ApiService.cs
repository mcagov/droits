using Droits.Exceptions;
using Droits.Models;
using Droits.Models.DTOs;

namespace Droits.Services;

public interface IApiService
{

    Task<SubmittedReportDto> SaveDroitReportAsync(SubmittedReportDto report);
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


    public Task<SubmittedReportDto> SaveDroitReportAsync(SubmittedReportDto report)
    {

        if ( report == null )
        {
            _logger.LogError("Report is null");
            throw new DroitNotFoundException();
        }
        
        // Generate reference for reporåt:
        report.Reference = $"{_random.NextInt64(1, 100)}/{DateTime.UtcNow:yy}";

        return Task.FromResult(report);
    }
}