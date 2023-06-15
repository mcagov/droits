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

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var emails = await _service.GetEmails();

        var emailViews = emails.Select(e => new EmailView(e)).ToList();

        var model = new EmailListView(emailViews);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Compose(Guid id)
    {
        EmailForm form;
        
        if (id != Guid.Empty)
        {
            Email email = await _service.GetEmailById(id);
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
    public async Task<IActionResult> SendEmail(Guid id, EmailForm form)
    {
        form.EmailId = id;
        await _service.SendEmailAsync(form);
            
        return View(nameof(SendEmail), form);
    }

    [HttpPost]
    public async Task<IActionResult> Preview(EmailForm form)
    {
        Guid emailId;

        if (form.EmailId != Guid.Empty)
        {
            emailId = form.EmailId;
            await _service.UpdateEmailAsync(form);
        }
        else
        {
            form.Body = form.GetEmailBody();
            emailId = _service.SaveEmail(form).Id;
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
    
    [HttpPut]
    public async Task<IActionResult> Edit(EmailForm form)
    {
        if (form.EmailId != Guid.Empty)
        {
            await _service.UpdateEmailAsync(form);
            return RedirectToAction(nameof(GetPreview), new { id = form.EmailId });
        }
        else
        {
            return new NotFoundResult();
        }
    }
}