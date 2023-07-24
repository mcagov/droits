using Droits.Clients;
using Droits.Exceptions;
using Droits.Models;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Models.FormModels;
using Droits.Models.ViewModels;
using Droits.Repositories;
using Notify.Models.Responses;

namespace Droits.Services;

public interface ILetterService
{

    Task<string> GetTemplateBodyAsync(LetterType letterType);
    Task<string> GetTemplateSubjectAsync(LetterType letterType);
    Task<EmailNotificationResponse> SendLetterAsync(Guid id);
    Task<Letter> GetLetterByIdAsync(Guid id);
    Task<Letter> SaveLetterAsync(LetterForm form);
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
                letter = await _letterRepository.AddLetterAsync(letter);
            }
            else
            {
                letter = await _letterRepository.UpdateLetterAsync(letter);
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
        letter.Body = SubstituteContentWithParams(letter.Body, model);
        letter.Subject = SubstituteContentWithParams(letter.Subject, model);

        return letter;
    }

    private string SubstituteContentWithParams(string content, LetterPersonalisationView model)
    {
        foreach ( var param in model.GetAsPersonalisation() )
        {
            content = content.Replace($"(({param.Key}))", param.Value);
        }

        return content;
    }

    public async Task<Letter> GetLetterByIdAsync(Guid id)
    {
        return await _letterRepository.GetLetterAsync(id);
    }


}
