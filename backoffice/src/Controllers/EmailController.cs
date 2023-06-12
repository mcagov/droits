using Droits.Models;
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
        _service.SaveEmailPreview(form);
        
        return View(nameof(Preview), form);
    }
    
    [HttpGet]
    public IActionResult GetLastModifiedPreview(string recipient)
    {
        // invoke service
        _service.GetEmailsForRecipient(recipient);
        
        // get email at top of the list
        
        // change Email to EmailForm

        // pass EmailForm to view
        return View(nameof(SendEmail), form);
    }
}