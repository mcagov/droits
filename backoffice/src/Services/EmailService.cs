using System.Text;
using Droits.Clients;
using Droits.Exceptions;
using Droits.Models;
using Droits.Models.Domain;
using Droits.Repositories;
using Notify.Models.Responses;

namespace Droits.Services;

public interface IEmailService
{
    Task<string> GetTemplateAsync(EmailType emailType);
    Task<EmailNotificationResponse> SendEmailAsync(EmailForm form);
    Task<Email> GetEmailById(Guid id);
    Email SaveEmail(EmailForm emailForm);
    Task UpdateEmailAsync(EmailForm emailForm);
    Task<List<Email>> GetEmails();
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
        string template;
        var filename = $"{TemplateDirectory}/{emailType.ToString()}.txt";
        
        using (StreamReader streamReader = new(filename, Encoding.UTF8))
        {
            template = await streamReader.ReadToEndAsync();
        }

        return template;    
    }

    
    public async Task<EmailNotificationResponse> SendEmailAsync(EmailForm form)
    {
        try
        {
            EmailNotificationResponse govNotifyResponse = await _client.SendEmailAsync(form);

            await MarkAsSent(form.EmailId);

            return govNotifyResponse;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }

    private async Task MarkAsSent(Guid id)
    {
        Email sentEmail = await GetEmailById(id);
            
        sentEmail.DateSent = DateTime.UtcNow;
        _emailRepository.UpdateEmailAsync(sentEmail);
    }

    public async Task<List<Email>> GetEmails()
    {
        return await _emailRepository.GetEmailsAsync();
    }
    
    public List<Email> GetEmailsForRecipient(string recipient)
    {
        return
            _emailRepository.GetEmails()
                .Where(e => e.Recipient.Equals(recipient))
                .OrderByDescending(e => e.DateLastModified)
                .ToList();
    }

    public Email SaveEmail(EmailForm emailForm)
    {
        DateTime todaysDate = DateTime.UtcNow;

        Email email = new()
        {
            Subject = emailForm.Subject,
            Body = emailForm.Body,
            Recipient = emailForm.EmailAddress,
            DateCreated = todaysDate,
            DateLastModified = todaysDate
        };

        try
        {
            return _emailRepository.AddEmail(email);
        }
        catch (Exception e)
        {
            _logger.LogError($"Error saving email: {e}");
            throw;
        }
    }
    
    public async Task UpdateEmailAsync(EmailForm emailForm)
    {
        if (emailForm.EmailId != Guid.Empty)
        {
            Email emailToUpdate = await GetEmailById(emailForm.EmailId);
        
            DateTime todaysDate = DateTime.UtcNow;

            emailToUpdate.Recipient = emailForm.EmailAddress;
            emailToUpdate.Subject = emailForm.Subject;
            emailToUpdate.Body = emailForm.Body;
            emailToUpdate.DateLastModified = todaysDate;
            
            try
            {
                _emailRepository.UpdateEmailAsync(emailToUpdate);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error updating email: {e}");
                throw;
            }
        }

        else
        {
            _logger.LogError("Email ID provided was null");
        }
    }
    
    public async Task<Email> GetEmailById(Guid id)
    {
        Email? email = await _emailRepository.GetEmailAsync(id);
        if (email == null)
        {
            throw new EmailNotFoundException();
        }
        
        return email;
    }
}