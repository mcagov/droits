using System.Text;
using Droits.Clients;
using Droits.Exceptions;
using Droits.Models;
using Droits.Models.Entities;
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
    
    private const string TemplateDirectory = "Views/Email/Templates" ;

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
        var filename = $"{TemplateDirectory}/{emailType.ToString()}.txt";

        using StreamReader streamReader = new(filename, Encoding.UTF8);
        
        var template = await streamReader.ReadToEndAsync();
        
        streamReader.Dispose();
        
        return template;    
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

    public async Task<List<Email>> GetEmailsAsync()
    {
        return await _emailRepository.GetEmailsAsync();
    }
    
    public async Task<List<Email>> GetEmailsForRecipientAsync(string recipient)
    {
        return _emailRepository.GetEmailsAsync()
                .Result
                .Where(e => e.Recipient.Equals(recipient))
                .OrderByDescending(e => e.DateLastModified)
                .ToList();
    }

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