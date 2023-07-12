using Droits.Models.FormModels;
using Notify.Client;
using Notify.Models.Responses;

namespace Droits.Clients;

public interface IGovNotifyClient
{
    Task<EmailNotificationResponse> SendEmailAsync(EmailForm form);
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

    public async Task<EmailNotificationResponse> SendEmailAsync(EmailForm form)
    {
        var personalisation = form.GetPersonalisation();
        personalisation.Add("body", form.GetEmailBody());

        return await _client.SendEmailAsync(
            form.Recipient,
            getTemplateId(),
            personalisation
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