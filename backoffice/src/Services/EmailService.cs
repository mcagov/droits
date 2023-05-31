
namespace Droits.Services
{
    using Droits.Clients;
    using Droits.Models;
    using Notify.Models.Responses;

    public interface IEmailService
    {
        string GetTemplateFilename(string fileLocation, EmailTemplateType templateType);
        Task<EmailNotificationResponse> SendEmailAsync(EmailForm form);
        TemplatePreviewResponse GetPreview(EmailForm form);
    }

    public class EmailService : IEmailService
    {
        private readonly IGovNotifyClient _client;
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger, IGovNotifyClient client){
            _logger = logger;
            _client = client;
        }

        public void getApiKey()
        {
            _client.getApiKey();
        }

        public string GetTemplateFilename(string fileLocation, EmailTemplateType templateType)
        {
            return $"{fileLocation}/{templateType.ToString()}.txt";
        }

        public async Task<EmailNotificationResponse> SendEmailAsync(EmailForm form) => await _client.SendEmailAsync(form);

        public TemplatePreviewResponse GetPreview(EmailForm form)
        {
            try
            {
                return _client.GetPreview(form);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

    }

}
