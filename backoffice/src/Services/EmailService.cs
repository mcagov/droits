namespace Droits.Services
{
    using Droits.Models;
    using Droits.Repositories;
    using Notify.Models.Responses;

    public interface IEmailService
{
    Task<EmailNotificationResponse> SendEmailAsync(EmailForm form);
}

    public class EmailService : IEmailService
    {
        private readonly IGovNotifyRepository _repository;
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger,IGovNotifyRepository repo){
            _logger = logger;
            _repository = repo;
        }

        public async Task<EmailNotificationResponse> SendEmailAsync(EmailForm form) => await _repository.SendEmailAsync(form);
    }

}
