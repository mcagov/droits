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
        {
            Dictionary<string,dynamic> personalisation = new Dictionary<string,dynamic>{
                { "body", form.Body},
                { "subject", form.Subject},
            };

            return await _client.SendEmailAsync(
                emailAddress: form.EmailAddress,
                templateId: getTemplateId(),
                personalisation: personalisation

            );
        }

        public string? getTemplateId(){
            var value =  _configuration.GetSection("GovNotify:TemplateId").Value;

            Console.WriteLine(value);
            return value;
        }

        public string? getApiKey(){
            var value =  _configuration.GetSection("GovNotify:ApiKey").Value;

            Console.WriteLine(value);

            return value;
        }
    }

}
