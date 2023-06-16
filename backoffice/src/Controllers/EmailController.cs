using Droits.Models;
using Droits.Models.Entities;
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
        var emails = await _service.GetEmailsAsync();

        var emailViews = emails.Select(e => new EmailView(e)).ToList();

        var model = new EmailListView(emailViews);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Compose(Guid id)
    {
        //This should be done elsewhere. 
        var emailType = EmailType.TestingDroitsv2;
        
        if (id == default(Guid))
        {
            return View(new EmailForm()
            {
                Body = await _service.GetTemplateAsync(emailType)
            });
        }
        
        var email = await _service.GetEmailByIdAsync(id);
        
        return View(new EmailForm(email));
    }

    [HttpGet]
    public async Task<IActionResult> SendEmail(Guid id)
    {
        await _service.SendEmailAsync(id);
        
        //Add feedback (banner or something) to show sent. 
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> SaveEmail(EmailForm form)
    {
        Email email; 
        
        if (form.EmailId == default(Guid))
        {
            email = await _service.SaveEmailAsync(form);
        }
        else
        {
            email = await _service.UpdateEmailAsync(form);
        }
        
        return RedirectToAction(nameof(Preview), new { id = email.Id });
    }
    
    [HttpGet]
    public async Task<IActionResult> Preview(Guid id)
    {
        var email = await _service.GetEmailByIdAsync(id);

        var model = new EmailView(email);

        return View(model);
    }
    
    [HttpPut]
    public async Task<IActionResult> Edit(EmailForm form)
    {
        if (form.EmailId != Guid.Empty)
        {
            await _service.UpdateEmailAsync(form);
            return RedirectToAction(nameof(Preview), new { id = form.EmailId });
        }
        else
        {
            return new NotFoundResult();
        }
    }
}