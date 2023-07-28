using Droits.Clients;
using Droits.Exceptions;
using Droits.Helpers;
using Droits.Models;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Models.FormModels;
using Droits.Models.ViewModels;
using Droits.Models.ViewModels.ListViews;
using Droits.Repositories;
using Microsoft.EntityFrameworkCore;
using Notify.Models.Responses;

namespace Droits.Services;

public interface ILetterService
{
    Task<LetterListView> GetLettersListViewAsync(SearchOptions searchOptions);
    Task<string> GetTemplateBodyAsync(LetterType letterType, Droit? droit);
    Task<string> GetTemplateSubjectAsync(LetterType letterType, Droit? droit);
    Task<EmailNotificationResponse> SendLetterAsync(Guid id);
    Task<Letter> GetLetterByIdAsync(Guid id);
    Task<Letter> SaveLetterAsync(LetterForm form);
    Task<List<Letter>> GetLettersAsync();
}

public class LetterService : ILetterService
{
    private readonly IGovNotifyClient _client;
    private readonly ILogger<LetterService> _logger;
    private readonly ILetterRepository _repo;
    private readonly IDroitService _droitService;
    private const string TemplateDirectory = "Views/LetterTemplates";


    public LetterService(ILogger<LetterService> logger,
        IGovNotifyClient client,
        ILetterRepository repo,
        IDroitService droitService)
    {
        _logger = logger;
        _client = client;
        _repo = repo;
        _droitService = droitService;
    }

    public async Task<LetterListView> GetLettersListViewAsync(SearchOptions searchOptions)
    {
        var query = searchOptions.IncludeAssociations ? _repo.GetLettersWithAssociations() : _repo.GetLetters();
        var pagedItems = await ServiceHelpers.GetPagedResult(query.Select(l => new LetterView(l)),searchOptions);

        return new LetterListView(pagedItems.Items)
        {
            PageNumber = pagedItems.PageNumber,
            PageSize = pagedItems.PageSize,
            IncludeAssociations = pagedItems.IncludeAssociations,
            TotalCount = pagedItems.TotalCount
        };
    }

    public async Task<string> GetTemplateBodyAsync(LetterType letterType, Droit? droit)
    {
        var templatePath = Path.Combine(Environment.CurrentDirectory, TemplateDirectory,
            $"{letterType.ToString()}.Body.txt");

        var content = await ReadFileContentAsync(templatePath);
  
        if ( droit == null )
        {
            return content;
        }

        return new LetterPersonalisationView(droit).SubstituteContent(content);
    }
    public async Task<string> GetTemplateSubjectAsync(LetterType letterType, Droit? droit)
    {
        var templatePath = Path.Combine(Environment.CurrentDirectory, TemplateDirectory,
            $"{letterType.ToString()}.Subject.txt");

        var content = await ReadFileContentAsync(templatePath);
  
        if ( droit == null )
        {
            return content;
        }
        
        return new LetterPersonalisationView(droit).SubstituteContent(content);
    }

    private async Task<string> ReadFileContentAsync(string path){
        if ( !File.Exists(path) )
        {
            _logger.LogError($"File could not be found at: {path}");
            throw new FileNotFoundException("File could not be found");
        }

        return await File.ReadAllTextAsync(path);
    }

    public async Task<EmailNotificationResponse> SendLetterAsync(Guid id)
    {
        try
        {
            var letter = await GetLetterByIdAsync(id);
            var govNotifyResponse = await _client.SendLetterAsync(letter);

            await MarkAsSentAsync(id);

            return govNotifyResponse;
        }
        catch ( Exception e )
        {
            _logger.LogError(e.Message);
            throw;
        }
    }


    private async Task MarkAsSentAsync(Guid id)
    {
        var sentLetter = await GetLetterByIdAsync(id);

        sentLetter.DateSent = DateTime.UtcNow;
        await _repo.UpdateLetterAsync(sentLetter);
    }


    public async Task<List<Letter>> GetLettersAsync()
    {
        return await _repo.GetLetters().ToListAsync();
    }
    

    public async Task<Letter> SaveLetterAsync(LetterForm form)
    {
        Letter letter;
        if ( form.Id == default )
        {
            letter = new()
            {
                DroitId = form.DroitId,
                Subject = form.Subject,
                Body = form.Body,
                Recipient = form.Recipient,
                Type = form.Type
            };
        }
        else
        {
            letter = await GetLetterByIdAsync(form.Id);
            letter = form.ApplyChanges(letter);
        }

        if ( letter.DroitId != default )
        {
            letter = await SubstituteLetterContentWithParamsAsync(letter);    
        }
        
        try
        {
            if ( form.Id == default )
            {
                letter = await _repo.AddLetterAsync(letter);
            }
            else
            {
                letter = await _repo.UpdateLetterAsync(letter);
            }
        }
        catch ( Exception e )
        {
            _logger.LogError($"Error saving letter: {e}");
            throw;
        }

        return letter;
    }

    private async Task<Letter> SubstituteLetterContentWithParamsAsync(Letter letter){

        var droit = await _droitService.GetDroitAsync(letter.DroitId);

        var model = new LetterPersonalisationView(droit);
        letter.Body = model.SubstituteContent(letter.Body);
        letter.Subject = model.SubstituteContent(letter.Subject);

        return letter;
    }

    public async Task<Letter> GetLetterByIdAsync(Guid id)
    {
        return await _repo.GetLetterAsync(id);
    }


}
