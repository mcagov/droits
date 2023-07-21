using Droits.Models.Entities;
using Droits.Models.FormModels;
using Notify.Client;
using Notify.Models.Responses;

namespace Droits.Clients;

public interface IGovNotifyClient
{
    Task<EmailNotificationResponse> SendLetterAsync(Letter form);
    string? getTemplateId();
    string? getApiKey();
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
        _client = new NotificationClient(getApiKey());
    }


    public async Task<EmailNotificationResponse> SendLetterAsync(Letter letter)
    {
        var data = new Dictionary<string, dynamic>();

        data.Add("body",letter.Body);
        data.Add("subject", letter.Subject);

        return await _client.SendEmailAsync(
            letter.Recipient,
            getTemplateId(),
            data
        );
    }


    public string getTemplateId()
    {
        return _configuration.GetSection("GovNotify:TemplateId").Value ?? "";
    }


    public string getApiKey()
    {
        return _configuration.GetSection("GovNotify:ApiKey").Value ?? "";
    }
}
