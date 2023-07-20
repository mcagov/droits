using System.Text;
using Droits.Clients;
using Droits.Exceptions;
using Droits.Models;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Models.FormModels;
using Droits.Repositories;
using Notify.Models.Responses;

namespace Droits.Services;

public interface ILetterService
{
    Task<string> GetTemplateAsync(LetterType letterType);
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

    private const string TemplateDirectory = "Views/LetterTemplates";


    public LetterService(ILogger<LetterService> logger,
        IGovNotifyClient client,
        ILetterRepository letterRepository)
    {
        _logger = logger;
        _client = client;
        _letterRepository = letterRepository;
    }


    public async Task<string> GetTemplateAsync(LetterType letterType)
    {
        var templatePath = Path.Combine(Environment.CurrentDirectory, TemplateDirectory,
            $"{letterType.ToString()}.txt");

        if ( !File.Exists(templatePath) )
        {
            _logger.LogError($"Template file could not be found at: {templatePath}");
            throw new FileNotFoundException("Template file could not be found");
        }

        return await File.ReadAllTextAsync(templatePath);
    }


    public async Task<EmailNotificationResponse> SendLetterAsync(Guid id)
    {
        try
        {
            var letter = await GetLetterByIdAsync(id);
            var govNotifyResponse = await _client.SendLetterAsync(new LetterForm(letter));

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


    public async Task<Letter> SaveLetterAsync(LetterForm letterForm)
    {
        Letter letter = new()
        {
            DroitId = letterForm.DroitId,
            Subject = letterForm.Subject,
            Body = letterForm.GetLetterBody(),
            Recipient = letterForm.Recipient
        };

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
        if ( letterForm.LetterId == default )
        {
            _logger.LogError("Letter with that ID does not exist");
            throw new LetterNotFoundException();
        }

        var letterToUpdate = await GetLetterByIdAsync(letterForm.LetterId);

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