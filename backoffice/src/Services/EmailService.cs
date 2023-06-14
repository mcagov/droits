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
    List<Email> GetEmailsForRecipient(string recipient);
    Task<Email> GetEmailById(Guid id);
    Email SaveEmailPreview(EmailForm emailForm);
    Task UpdateEmailPreviewAsync(EmailForm emailForm);
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


    // add date sent
    // save in repo
    public async Task<EmailNotificationResponse> SendEmailAsync(EmailForm form) => await _client.SendEmailAsync(form);

    public async Task<Email> GetEmailById(Guid id)
    {
        Email? email = await _emailRepository.GetEmailAsync(id);
        if (email == null)
        {
            throw new EmailNotFoundException();
        }
        
        return email;
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

    public Email SaveEmailPreview(EmailForm emailForm)
    {
        Email emailPreview = new();
        DateTime todaysDate = DateTime.UtcNow;
        
        emailPreview.Subject = emailForm.Subject;
        emailPreview.Body = emailForm.Body;
        emailPreview.Recipient = emailForm.EmailAddress;
        emailPreview.DateCreated = todaysDate;
        emailPreview.DateLastModified = todaysDate;

        try
        {
            return _emailRepository.AddEmail(emailPreview);
        }
        catch (Exception e)
        {
            _logger.LogError($"Error saving email preview: {e}");
            throw;
        }
    }
    
    public async Task UpdateEmailPreviewAsync(EmailForm emailForm)
    {
        if (emailForm.EmailId.HasValue)
        {
            Email emailToUpdate = await _emailRepository.GetEmailAsync(emailForm.EmailId.Value);
            if (emailToUpdate == null)
            {
                throw new EmailNotFoundException();
            }
        
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
                _logger.LogError($"Error updating email preview: {e}");
                throw;
            }
        }

        else
        {
            _logger.LogError("Email ID provided was null");
        }
    }
}