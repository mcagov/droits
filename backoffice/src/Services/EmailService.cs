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
    Task<Email> SaveEmailPreview(EmailForm emailForm);
}

public class EmailService : IEmailService
{
    private readonly IGovNotifyClient _client;
    private readonly ILogger<EmailService> _logger;
    private readonly IEmailRepository _emailRepository;
    
    private const string TemplateDirectory = "Views/Email/Templates" ;
    private const string ReplyToEmailAddress = "row@mcga.gov.uk";

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
    
    public List<Email> GetEmailsForRecipient(string recipient)
    {
        return
            _emailRepository.GetEmails()
                .Where(e => e.Recipient.Equals(recipient))
                .OrderByDescending(e => e.DateLastModified)
                .ToList();
    }

    public async Task<Email> SaveEmailPreview(EmailForm emailForm)
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
            return await _emailRepository.AddEmailAsync(emailPreview);
        }
        catch (Exception e)
        {
            _logger.LogError($"Error saving email preview: {e}");
            throw;
        }
    }
}