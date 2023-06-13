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

        var model = emails.Select(e => new EmailView(e)).ToList();
        
        return View(model);
    }

    public async Task<IActionResult> Compose()
    {
        var model = new EmailForm()
        {
            Body = await _service.GetTemplateAsync(EmailType.TestingDroitsv2)
        };
        
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> SendEmail(EmailForm form)
    {
        var result = await _service.SendEmailAsync(form);
            
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
        
        EmailForm form = new();
        form.Body = email.Body;
        form.Subject = email.Subject;
        form.EmailAddress = email.Recipient;

        return View(nameof(Preview), form);
    }
}