using System.Text;

using Droits.Clients;
using Droits.Exceptions;
using Droits.Models;
using Droits.Repositories;
using Notify.Models.Responses;

namespace Droits.Services;

public interface IEmailService
{
    EmailForm GetEmailForm(string fileLocation, EmailTemplateType templateType);
    string GetTemplateFilename(string fileLocation, EmailTemplateType templateType);
    Task<EmailNotificationResponse> SendEmailAsync(EmailForm form);
    TemplatePreviewResponse GetPreview(EmailForm form);
}


public class EmailService : IEmailService
{
    private readonly IGovNotifyClient _client;
    private readonly ILogger<EmailService> _logger;
    private readonly IEmailRepository _emailRepository;

    public EmailService(ILogger<EmailService> logger, 
        IGovNotifyClient client,
        IEmailRepository emailRepository)
    {
        _logger = logger;
        _client = client;
        _emailRepository = emailRepository;
    }

    public EmailForm GetEmailForm(string fileLocation, EmailTemplateType templateType)
    {
        string template;
        var filename = GetTemplateFilename(fileLocation, templateType);
        using (StreamReader streamReader = new(filename, Encoding.UTF8))
        {
            template = streamReader.ReadToEnd();
        }

        return new EmailForm
        {
            Body = template
        };
    }

    public string GetTemplateFilename(string fileLocation, EmailTemplateType templateType)
    {
        return $"{fileLocation}/{templateType.ToString()}.txt";
    }

    public async Task<EmailNotificationResponse> SendEmailAsync(EmailForm form)
    {
        return await _client.SendEmailAsync(form);
    }

    public TemplatePreviewResponse GetPreview(EmailForm form)
    {
        try
        {
            var preview = _client.GetPreview(form);
            return preview;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }

    public void GetApiKey()
    {
        _client.getApiKey();
    }

    public async Task<Email> GetEmailByIdAsync(Guid id)
    {
        Email? email = await _emailRepository.GetEmailAsync(id);
        
        if (email == null)
        {
            throw new EmailNotFoundException();
        }

        return email;
    }
}