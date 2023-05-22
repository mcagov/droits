namespace Droits.Repositories
{
    using Droits.Models;
    using Notify.Client;
    using Notify.Models.Responses;

    public interface IGovNotifyRepository
{
    Task<EmailNotificationResponse> SendEmailAsync(EmailForm form);
}

    public class GovNotifyRepository : IGovNotifyRepository
    {
        private readonly NotificationClient _client;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GovNotifyRepository> _logger;


        public GovNotifyRepository(ILogger<GovNotifyRepository> logger,IConfiguration configuration){
            _logger = logger;
            _configuration = configuration;
            _client = new NotificationClient(_configuration.GetSection("GovNotify:ApiKey").Value);
        }

        public async Task<EmailNotificationResponse> SendEmailAsync(EmailForm form)
        {
            string? templateId = _configuration.GetSection("GovNotify:TemplateId").Value;

            Dictionary<string,dynamic> personalisation = new Dictionary<string,dynamic>{
                { "body", form.Body},
                { "subject", form.Subject},
            };

            return await _client.SendEmailAsync(
                emailAddress: form.EmailAddress,
                templateId: templateId,
                personalisation: personalisation

            );
        }
    }

}
