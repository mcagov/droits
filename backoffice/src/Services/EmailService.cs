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

public interface IEmailService
{
    Task<string> GetTemplateAsync(EmailType emailType);
    Task<EmailNotificationResponse> SendEmailAsync(Guid id);
    Task<Email> GetEmailByIdAsync(Guid id);
    Task<Email> SaveEmailAsync(EmailForm emailForm);
    Task<Email> UpdateEmailAsync(EmailForm emailForm);
    Task<List<Email>> GetEmailsAsync();
}

public class EmailService : IEmailService
{
    private readonly IGovNotifyClient _client;
    private readonly ILogger<EmailService> _logger;
    private readonly IEmailRepository _emailRepository;

    private const string TemplateDirectory = "Views/EmailTemplates" ;

    public EmailService(ILogger<EmailService> logger,
        IGovNotifyClient client,
        IEmailRepository emailRepository)
    {
        _logger = logger;
        _client = client;
        _emailRepository = emailRepository;
    }

    public async Task<string> GetTemplateAsync(EmailType emailType)
    {
        var templatePath = Path.Combine(Environment.CurrentDirectory,TemplateDirectory,$"{emailType.ToString()}.txt");

        if(!File.Exists(templatePath)){
            _logger.LogError($"Template file could not be found at: {templatePath}");
            throw new FileNotFoundException("Template file could not be found");
        }

        return await File.ReadAllTextAsync(templatePath);
    }


    public async Task<EmailNotificationResponse> SendEmailAsync(Guid id)
    {
        try
        {
            var email = await GetEmailByIdAsync(id);
            var govNotifyResponse = await _client.SendEmailAsync(new EmailForm(email));

            await MarkAsSentAsync(id);

            return govNotifyResponse;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }

    private async Task MarkAsSentAsync(Guid id)
    {
        var sentEmail = await GetEmailByIdAsync(id);

        sentEmail.DateSent = DateTime.UtcNow;
        await _emailRepository.UpdateEmailAsync(sentEmail);
    }

    public async Task<List<Email>> GetEmailsAsync() =>
        await _emailRepository.GetEmailsAsync();

    public async Task<List<Email>> GetEmailsForRecipientAsync(string recipient) =>
        await _emailRepository.GetEmailsForRecipientAsync(recipient);

    public async Task<Email> SaveEmailAsync(EmailForm emailForm)
    {
        Email email = new()
        {
            Subject = emailForm.Subject,
            Body = emailForm.GetEmailBody(),
            Recipient = emailForm.Recipient,
        };

        try
        {
            return await _emailRepository.AddEmailAsync(email);
        }
        catch (Exception e)
        {
            _logger.LogError($"Error saving email: {e}");
            throw;
        }
    }

    public async Task<Email> UpdateEmailAsync(EmailForm emailForm)
    {

        if (emailForm.EmailId == default(Guid))
        {
            _logger.LogError("Email with that ID does not exist");
            throw new EmailNotFoundException();
        }

        var emailToUpdate = await GetEmailByIdAsync(emailForm.EmailId);

        emailToUpdate = emailForm.ApplyChanges(emailToUpdate);

        try
        {
            return await _emailRepository.UpdateEmailAsync(emailToUpdate);
        }
        catch (Exception e)
        {
            _logger.LogError($"Error updating email: {e}");
            throw;
        }
    }

    public async Task<Email> GetEmailByIdAsync(Guid id) =>
        await _emailRepository.GetEmailAsync(id);

}
