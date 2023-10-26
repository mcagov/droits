using Droits.Clients;
using Droits.Helpers;
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
    Task<Letter> SendLetterAsync(Guid id);
    Task<Letter> GetLetterAsync(Guid id);
    Task<Letter> SaveLetterAsync(LetterForm form);
    Task<LetterListView> GetApprovedUnsentLettersListViewForCurrentUserAsync(SearchOptions searchOptions);
}

public class LetterService : ILetterService
{
    private readonly IGovNotifyClient _client;
    private readonly ILogger<LetterService> _logger;
    private readonly ILetterRepository _repo;
    private readonly IDroitService _droitService;
    private readonly IAccountService _accountService;
    private const string TemplateDirectory = "Views/LetterTemplates";


    public LetterService(ILogger<LetterService> logger,
        IGovNotifyClient client,
        ILetterRepository repo,
        IDroitService droitService,
        IAccountService accountService)
    {
        _logger = logger;
        _client = client;
        _repo = repo;
        _droitService = droitService;
        _accountService = accountService;
    }


    public async Task<LetterListView> GetLettersListViewAsync(SearchOptions searchOptions)
    {
        var query = GetLetterQuery(searchOptions);

        query = query.Where(l => l.DateSent == null);
        query = query.OrderBy(l =>
                l.Status == LetterStatus.ReadyForQC ? 0 :
                l.Status == LetterStatus.ActionRequired ? 1 :
                l.Status == LetterStatus.QCApproved ? 2 :
                3 // Draft
        ).ThenByDescending(l => l.Created);
        var pagedItems =
            await ServiceHelper.GetPagedResult(query.Select(l => new LetterView(l, searchOptions.IncludeAssociations)),
                searchOptions);

        return new LetterListView(pagedItems.Items)
        {
            PageNumber = pagedItems.PageNumber,
            PageSize = pagedItems.PageSize,
            IncludeAssociations = pagedItems.IncludeAssociations,
            TotalCount = pagedItems.TotalCount
        };
    }


    private IQueryable<Letter> GetLetterQuery(SearchOptions searchOptions)
    {
        return searchOptions.IncludeAssociations
            ? _repo.GetLettersWithAssociations()
            : _repo.GetLetters();
    }
    public async Task<LetterListView> GetApprovedUnsentLettersListViewForCurrentUserAsync(SearchOptions searchOptions)
    {
        var currentUserId = _accountService.GetCurrentUserId();
        var query = GetLetterQuery(searchOptions);

        query = query.Where(l =>
            l.DateSent == null &&
            l.Status == LetterStatus.QCApproved &&
            (l.Droit != null && l.Droit.AssignedToUserId == currentUserId )
        );
        
        var pagedItems =
            await ServiceHelper.GetPagedResult(query.Select(l => new LetterView(l, searchOptions.FilterByAssignedUser)),
                searchOptions);
        
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


    private async Task<string> ReadFileContentAsync(string path)
    {
        if ( File.Exists(path) ) return await File.ReadAllTextAsync(path);
        
        
        _logger.LogError($"File could not be found at: {path}");
        return "";
        
    }


    public async Task<Letter> SendLetterAsync(Guid id)
    {
        try
        {
            var letter = await GetLetterAsync(id);
            await _client.SendLetterAsync(letter);

            await MarkAsSentAsync(id);

            return letter;
        }
        catch ( Exception e )
        {
            _logger.LogError(e.Message);
            throw;
        }
    }


    private async Task MarkAsSentAsync(Guid id)
    {
        var sentLetter = await GetLetterAsync(id);

        sentLetter.DateSent = DateTime.UtcNow;
        sentLetter.Status = LetterStatus.Sent;
        await _repo.UpdateAsync(sentLetter);
    }

    public async Task<Letter> SaveLetterAsync(LetterForm form)
    {
        Letter letter;
        var status = LetterStatus.Draft;
        
        if ( form.Id == default )
        {
            letter = new Letter
            {
                DroitId = form.DroitId,
                Subject = form.Subject,
                Body = form.Body,
                Recipient = form.Recipient,
                Type = form.Type,
                Status = form.Status
            };
        }
        else
        {
            letter = await GetLetterAsync(form.Id);
            status = letter.Status;
                
            letter = form.ApplyChanges(letter);
        }
        
        
        if ( status != LetterStatus.QCApproved && form.Status == LetterStatus.QCApproved )
        {
            var currentUserId = _accountService.GetCurrentUserId();
            letter.QualityApprovedUserId = currentUserId;
        }

        if ( letter.DroitId != default )
        {
            letter = await SubstituteLetterContentWithParamsAsync(letter);
        }

        try
        {
            if ( form.Id == default )
            {
                letter = await _repo.AddAsync(letter);
            }
            else
            {
                letter = await _repo.UpdateAsync(letter);
            }
        }
        catch ( Exception e )
        {
            _logger.LogError($"Error saving letter: {e}");
            throw;
        }

        return letter;
    }


    private async Task<Letter> SubstituteLetterContentWithParamsAsync(Letter letter)
    {
        var droit = await _droitService.GetDroitAsync(letter.DroitId);

        var model = new LetterPersonalisationView(droit);
        letter.Body = model.SubstituteContent(letter.Body);
        letter.Subject = model.SubstituteContent(letter.Subject);

        return letter;
    }


    public async Task<Letter> GetLetterAsync(Guid id)
    {
        return await _repo.GetLetterAsync(id);
    }
}
