using System.Text;
using Droits.Clients;
using Droits.Models;
using Notify.Models.Responses;

namespace Droits.Services;

public interface IEmailService
{
    Task<string> GetTemplateAsync(EmailType emailType);
    Task<EmailNotificationResponse> SendEmailAsync(EmailForm form);
}

public class EmailService : IEmailService
{
    private readonly IGovNotifyClient _client;
    private readonly ILogger<EmailService> _logger;
    private const string TemplateDirectory = "Views/Email/Templates" ;
    public EmailService(ILogger<EmailService> logger, IGovNotifyClient client)
    {
        _logger = logger;
        _client = client;
    }

    public async Task<string> GetTemplateAsync(EmailType emailType)
    {
        // abstract out
        string template;
        var filename = $"{TemplateDirectory}/{emailType.ToString()}.txt";
        
        using (StreamReader streamReader = new(filename, Encoding.UTF8))
        {
            template = await streamReader.ReadToEndAsync();
        }

        return template;    
    }


    public async Task<EmailNotificationResponse> SendEmailAsync(EmailForm form) => await _client.SendEmailAsync(form);
}

