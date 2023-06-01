using Droits.Models;

namespace Droits.Clients
{
    using Droits.Models.Email;
    using Notify.Client;
    using Notify.Models.Responses;

    public interface IGovNotifyClient
{
    Task<EmailNotificationResponse> SendEmailAsync(EmailForm form);
    TemplatePreviewResponse GetPreview(EmailForm form);
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
                    personalisation: form.GetPersonalisation()
                );

        public TemplatePreviewResponse GetPreview(EmailForm form)
            =>  _client.GenerateTemplatePreview(
                templateId: getTemplateId(), 
                personalisation: form.GetPersonalisation()
            );
        

        public string getTemplateId() => _configuration.GetSection("GovNotify:TemplateId").Value ?? "";
        public string getApiKey() => _configuration.GetSection("GovNotify:ApiKey").Value ?? "";
    }

}
