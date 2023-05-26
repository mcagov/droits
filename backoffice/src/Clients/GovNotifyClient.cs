namespace Droits.Clients
{
    using Droits.Models;
    using Notify.Client;
    using Notify.Models.Responses;

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


        public GovNotifyClient(ILogger<GovNotifyClient> logger,IConfiguration configuration){
            _logger = logger;
            _configuration = configuration;
            _client = new NotificationClient(getApiKey());
        }

        public async Task<EmailNotificationResponse> SendEmailAsync(EmailForm form)
            => await _client.SendEmailAsync(
                    emailAddress: form.EmailAddress,
                    templateId: getTemplateId(),
                    personalisation: form.getPersonalisation()
                );

        public string getTemplateId() => _configuration.GetSection("GovNotify:TemplateId").Value ?? "";
        public string getApiKey() => _configuration.GetSection("GovNotify:ApiKey").Value ?? "";
    }

}
