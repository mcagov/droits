using Droits.Clients;
using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Models.FormModels;
using Droits.Repositories;
using Notify.Models.Responses;

namespace Droits.Services;

public interface ILetterService
{

    Task<string> GetTemplateBodyAsync(LetterType letterType);
    Task<string> GetTemplateSubjectAsync(LetterType letterType);
    Task<EmailNotificationResponse> SendLetterAsync(Guid id);
    Task<Letter> GetLetterByIdAsync(Guid id);
    Task<Letter> SaveLetterAsync(LetterForm letterForm);
    Task<Letter> UpdateLetterAsync(LetterForm letterForm);
    Task<List<Letter>> GetLettersAsync();
}

public class LetterService : ILetterService
{
    private readonly IGovNotifyClient _client;
    private readonly ILogger<LetterService> _logger;
    private readonly ILetterRepository _letterRepository;
    private readonly IDroitService _droitService;
    private const string TemplateDirectory = "Views/LetterTemplates";


    public LetterService(ILogger<LetterService> logger,
        IGovNotifyClient client,
        ILetterRepository letterRepository,
        IDroitService droitService)
    {
        _logger = logger;
        _client = client;
        _letterRepository = letterRepository;
        _droitService = droitService;
    }


    public async Task<string> GetTemplateBodyAsync(LetterType letterType)
    {
        var templatePath = Path.Combine(Environment.CurrentDirectory, TemplateDirectory,
            $"{letterType.ToString()}.Body.txt");

        return await ReadFileContentAsync(templatePath);
    }
    public async Task<string> GetTemplateSubjectAsync(LetterType letterType)
    {
        var templatePath = Path.Combine(Environment.CurrentDirectory, TemplateDirectory,
            $"{letterType.ToString()}.Subject.txt");

        return await ReadFileContentAsync(templatePath);

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
        await _letterRepository.UpdateLetterAsync(sentLetter);
    }


    public async Task<List<Letter>> GetLettersAsync()
    {
        return await _letterRepository.GetLettersAsync();
    }


    public async Task<List<Letter>> GetLettersForRecipientAsync(string recipient)
    {
        return await _letterRepository.GetLettersForRecipientAsync(recipient);
    }


    private async Task<Letter> SubstituteLetterContentWithParamsAsync(Letter letter){

        var droit = await _droitService.GetDroitAsync(letter.DroitId);

        letter.Body = SubstituteContentWithParams(letter.Body, droit);
        letter.Subject = SubstituteContentWithParams(letter.Subject, droit);

        return letter;
    }

    private string SubstituteContentWithParams(string content, Droit droit)
    {
        foreach ( var param in GetPersonalisation(droit) )
        {
            content = content.Replace($"(({param.Key}))", param.Value);
        }

        return content;
    }

    //TODO - This is not great - should be an object with fixed properties. Middle ground between what it was and what we want.
    //Also perfect example of where TDD should be used.
    private Dictionary<string, dynamic> GetPersonalisation(Droit droit)
    {
        return new Dictionary<string, dynamic>
        {
            { "email address", droit?.Salvor?.Email??"(Salvor Email Address)"},
            { "reference", droit?.Reference??"(Reference)" },
            { "custom message", "This is no longer used, as can edit on the fly..." },
            { "item pluralised", "*item pluralised (TBC)*" },
            { "items", "*items* (TBC)" },
            { "this pluralised", "*this pluralised* (TBC)" },
            { "wreck", droit?.Wreck?.Name??"(Wreck Name)" },
            { "date", "*date* (TBC)" },
            { "has pluralised", "*has pluralised* (TBC)" },
            { "link_to_file", "*link to file* (TBC)" },
            { "is pluralised", "*is pluralised* (TBC)" }
        };
    }
    public async Task<Letter> SaveLetterAsync(LetterForm letterForm)
    {


        Letter letter = new()
        {
            DroitId = letterForm.DroitId,
            Subject = letterForm.Subject,
            Body = letterForm.Body,
            Recipient = letterForm.Recipient,
            Type = letterForm.Type
        };

        letter = await SubstituteLetterContentWithParamsAsync(letter);

        try
        {
            return await _letterRepository.AddLetterAsync(letter);
        }
        catch ( Exception e )
        {
            _logger.LogError($"Error saving letter: {e}");
            throw;
        }
    }


    public async Task<Letter> UpdateLetterAsync(LetterForm letterForm)
    {
        if ( letterForm.Id == default )
        {
            _logger.LogError("Letter with that ID does not exist");
            throw new LetterNotFoundException();
        }

        var letterToUpdate = await GetLetterByIdAsync(letterForm.Id);

        letterToUpdate = letterForm.ApplyChanges(letterToUpdate);

        try
        {
            return await _letterRepository.UpdateLetterAsync(letterToUpdate);
        }
        catch ( Exception e )
        {
            _logger.LogError($"Error updating letter: {e}");
            throw;
        }
    }


    public async Task<Letter> GetLetterByIdAsync(Guid id)
    {
        return await _letterRepository.GetLetterAsync(id);
    }


}
