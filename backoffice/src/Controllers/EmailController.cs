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
    public async Task<IActionResult> Preview(EmailForm form)
    {
        form.Body = form.GetEmailBody();
        Email savedPreview = await _service.SaveEmailPreview(form);
        
        // get the freshly made preview here by id
        // EmailForm previewForm = _service.GetEmailById(id);
        
        return View(nameof(Preview), form);
    }
}