using Droits.Models;
using Droits.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Droits.Services;


namespace Droits.Controllers;

public class EmailController : Controller
{
    private readonly ILogger<EmailController> _logger;
    private readonly IEmailService _service;

    public EmailController(ILogger<EmailController> logger, IEmailService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var emails = await _service.GetEmails();

        var emailViews = emails.Select(e => new EmailView(e)).ToList();

        var model = new EmailListView(emailViews);
        return View(model);
    }

    public async Task<IActionResult> Compose(Guid? id)
    {
        EmailForm form;
        
        if (id.HasValue)
        {
            Email email = await _service.GetEmailById(id.Value);
            form = new()
            {
                EmailId = email.Id,
                EmailAddress = email.Recipient,
                Subject = email.Subject,
                Body = email.Body
            };
        }
        else
        {
            form = new()
            {
                Body = await _service.GetTemplateAsync(EmailType.TestingDroitsv2)
            };
        }

        return View(form);
    }

    [HttpPost]
    public async Task<IActionResult> SendEmail(EmailForm form)
    {
        await _service.SendEmailAsync(form);
            
        return View(nameof(SendEmail), form);
    }

    [HttpPost]
    public async Task<IActionResult> Preview(EmailForm form)
    {
        Guid emailId;

        if (form.EmailId.HasValue)
        {
            emailId = form.EmailId.Value;
            await _service.UpdateEmailPreviewAsync(form);
        }
        else
        {
            form.Body = form.GetEmailBody();
            emailId = _service.SaveEmailPreview(form).Id;
        }

        return RedirectToAction(nameof(GetPreview), new { id = emailId });
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPreview(Guid id)
    {
        Email email = await _service.GetEmailById(id);
        
        EmailForm form = new()
        {
            EmailId = email.Id,
            EmailAddress = email.Recipient,
            Subject = email.Subject,
            Body = email.Body
        };

        return View(nameof(Preview), form);
    }
    
    public async Task<IActionResult> Edit(EmailForm form)
    {
        if (form.EmailId.HasValue)
        {
            await _service.UpdateEmailPreviewAsync(form);
            return RedirectToAction(nameof(GetPreview), new { id = form.EmailId.Value });
        }
        else
        {
            return new NotFoundResult();
        }
    }
}