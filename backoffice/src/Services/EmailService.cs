
using System.Text;

namespace Droits.Services
{
    using Droits.Clients;
    using Droits.Models;
    using Notify.Models.Responses;

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

        public EmailService(ILogger<EmailService> logger, IGovNotifyClient client){
            _logger = logger;
            _client = client;
        }

        public void GetApiKey()
        {
            _client.getApiKey();
        }

        public EmailForm GetEmailForm(string fileLocation, EmailTemplateType templateType)
        {
            // abstract out
            string template;
            string filename = GetTemplateFilename(fileLocation, templateType);
            using (StreamReader streamReader = new(filename, Encoding.UTF8))
            {
                template = streamReader.ReadToEnd();
            }

            return new EmailForm(){
                Body = template
            };
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
                TemplatePreviewResponse preview = _client.GetPreview(form);
                preview.subject = "<h1>Hello</h1>";
                return preview;

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
    }

}
