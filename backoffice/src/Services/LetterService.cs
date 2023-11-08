using System.Text.RegularExpressions;
using Droits.Clients;
using Droits.Exceptions;
using Droits.Helpers;
using Droits.Helpers.Extensions;
using Droits.Models.DTOs;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Models.FormModels;
using Droits.Models.FormModels.SearchFormModels;
using Droits.Models.ViewModels;
using Droits.Models.ViewModels.ListViews;
using Droits.Repositories;
using Microsoft.IdentityModel.Tokens;

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
    
    Task<LetterListView> AdvancedSearchAsync(LetterSearchForm form);

    Task SendSubmissionConfirmationEmailAsync(Droit droit, SubmittedReportDto report);
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
    
    public async Task<LetterListView> AdvancedSearchAsync(LetterSearchForm form)
    {
        var query = _repo.GetLettersWithAssociations()
            .OrderBy(l =>
                    l.Status == LetterStatus.ReadyForQC ? 0 :
                    l.Status == LetterStatus.ActionRequired ? 1 :
                    l.Status == LetterStatus.QCApproved ? 2 :
                    3 // Draft
            ).ThenByDescending(l => l.Created)
            .Where(l =>
                SearchHelper.FuzzyMatches(form.Recipient, l.Recipient, 70) && 
                ( form.StatusList.IsNullOrEmpty() ||
                  form.StatusList.Contains(l.Status) ) &&
                ( form.TypeList.IsNullOrEmpty() ||
                  form.TypeList.Contains(l.Type) ) 
            )
            .Select(l => new LetterView(l, true));
        
        var pagedResults =
            await ServiceHelper.GetPagedResult(query, form);

        return new LetterListView(pagedResults.Items)
        {
            PageNumber = pagedResults.PageNumber,
            PageSize = pagedResults.PageSize,
            IncludeAssociations = pagedResults.IncludeAssociations,
            TotalCount = pagedResults.TotalCount,
            SearchForm = form
        };
    }


    public async Task SendSubmissionConfirmationEmailAsync(Droit droit, SubmittedReportDto report)
    {

        var salvor = droit.Salvor;

        if ( salvor == null )
        {
            throw new SalvorNotFoundException();
        }

        var confirmationLetter = new Letter
        {
            DroitId = droit.Id,
            Recipient = salvor.Email ?? "",
            Type = LetterType.ReportConfirmed
        };

        confirmationLetter.Subject = await GetTemplateSubjectAsync(confirmationLetter.Type, droit);

        if ( droit == null )
        {
            throw new DroitNotFoundException();
        }
        

        confirmationLetter.Body = await GetReportConfirmedEmailBodyAsync(droit, report);

        confirmationLetter = await _repo.AddAsync(confirmationLetter);
        await SendLetterAsync(confirmationLetter.Id); // Don't actually send it at the moment 


        _logger.LogInformation(
            $"Sending confirmation email to {salvor?.Email} for Droit {droit?.Reference}");
    }
private async Task<string> GetReportConfirmedEmailBodyAsync(Droit droit, SubmittedReportDto report)
{
    var templateBody = await GetTemplateBodyAsync(LetterType.ReportConfirmed, droit);

    var submittedWreckName = report.VesselName.ValueOrEmpty();

    var itemsReportedSection = BuildItemsReportedSection(droit, submittedWreckName ?? string.Empty);

    templateBody = Regex.Replace(templateBody, @"\(\(items_reported_section\)\)", itemsReportedSection);

    var isLateSubmission = true || droit.ReportedDate.Subtract(droit.DateFound).Days > 28;
    var lateReportSection = BuildLateReportSection(isLateSubmission);

    templateBody = Regex.Replace(templateBody, @"\(\(late_report_section\)\)", lateReportSection);

    var yourResponsibilitiesSection = BuildYourResponsibilitiesSection(droit.WreckMaterials.Count);

    templateBody = Regex.Replace(templateBody, @"\(\(your_responsibilities_section\)\)", yourResponsibilitiesSection);

    return templateBody;
}

private static string BuildItemsReportedSection(Droit droit, string submittedWreckName)
{
    var itemsReportedSection = $@"
        The following {"item has".Pluralize(droit.WreckMaterials.Count, "items have")} been reported {(!string.IsNullOrEmpty(submittedWreckName) ? $"from the wreck named {submittedWreckName}" : "")} :
";
    itemsReportedSection += " \n";
    itemsReportedSection = droit.WreckMaterials.Aggregate(itemsReportedSection, (current, item) => current + $"\n - {item.Name}");

    return itemsReportedSection;
}

private static string BuildLateReportSection(bool isLateSubmission)
{
    var lateReportSection = "";

    if ( !isLateSubmission ) return lateReportSection;
    
    lateReportSection += @"#Report submitted late";
    lateReportSection += @"
            You have not submitted this Report of wreck material within 28 days of the wreck material being recovered. For all future recoveries, please note that all recovered wreck material should be reported to the Receiver of Wreck within 28 days of recovery.";

    return lateReportSection;
}

private static string BuildYourResponsibilitiesSection(int itemCount)
{
    var yourResponsibilitiesSection = $@"
            The recovered {"item is".Pluralize(itemCount, "items are")} now held by you on indemnity to the Receiver of Wreck whilst investigations into legal ownership are undertaken.

            You do not own the reported {"item".Pluralize(itemCount, "items")} until advised by the Receiver of Wreck. You must not sell or otherwise dispose of the {"item".Pluralize(itemCount, "items")} until advised by the Receiver of Wreck.

            All finds from a maritime context are subject to various physical and chemical processes once raised. Conservators advise that finds should be kept wet, cool and dark until more specific advice can be sought.
";

    return yourResponsibilitiesSection;
}

}
