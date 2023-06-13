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

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Compose()
    {
        EmailForm form = new EmailForm()
        {
            Body = await _service.GetTemplateAsync(EmailType.TestingDroitsv2)
        };
        
        return View(form);
    }
    
    public async Task<IActionResult> Edit(Guid id)
    {
        Email email = await _service.GetEmailById(id);
        EmailForm form = new()
        {
            EmailAddress = email.Recipient,
            Subject = email.Subject,
            Body = email.Body
        };
        
        form.Body = await _service.GetTemplateAsync(EmailType.TestingDroitsv2);

        return View(nameof(Compose), form);
    }

    [HttpPost]
    public async Task<IActionResult> SendEmail(EmailForm form)
    {
        await _service.SendEmailAsync(form);
            
        return View(nameof(SendEmail), form);
    }

    [HttpPost]
    public IActionResult Preview(EmailForm form)
    {
        form.Body = form.GetEmailBody();
        Email savedPreview = _service.SaveEmailPreview(form);

        return RedirectToAction(nameof(GetPreview), new { id = savedPreview.Id });
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPreview(Guid id)
    {
        Email email = await _service.GetEmailById(id);
        
        // instead of EmailForm we should have a data structure view model
        // the view can't deal with stuff like .GetBody()
        // we need a Preview view model that has Id on it
        EmailForm form = new()
        {
            EmailId = email.Id,
            EmailAddress = email.Recipient,
            Subject = email.Subject,
            Body = email.Body
        };

        return View(nameof(Preview), form);
    }
}