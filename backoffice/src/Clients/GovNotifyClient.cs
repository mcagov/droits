using Droits.Models.Entities;
using Droits.Models.FormModels;
using Notify.Client;
using Notify.Models.Responses;

namespace Droits.Clients;

public interface IGovNotifyClient
{
    Task<EmailNotificationResponse> SendLetterAsync(Letter letter);
    string? GetTemplateId();
    string? GetApiKey();
}

public class GovNotifyClient : IGovNotifyClient
{
    private readonly NotificationClient _client;
    private readonly IConfiguration _configuration;
    private readonly ILogger<GovNotifyClient> _logger;


    public GovNotifyClient(ILogger<GovNotifyClient> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _client = new NotificationClient(GetApiKey());
    }


    public async Task<EmailNotificationResponse> SendLetterAsync(Letter letter)
    {
        var data = new Dictionary<string, dynamic>
        {
            { "body", letter.Body },
            { "subject", letter.Subject }
        };

        return await _client.SendEmailAsync(
            letter.Recipient,
            GetTemplateId(),
            data
        );
    }


    public string GetTemplateId()
    {
        return _configuration.GetSection("GovNotify:TemplateId").Value ?? "";
    }


    public string GetApiKey()
    {
        return _configuration.GetSection("GovNotify:ApiKey").Value ?? "";
    }
}
