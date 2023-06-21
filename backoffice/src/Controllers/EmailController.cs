using Droits.Models;
using Droits.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Droits.Services;


namespace Droits.Controllers;

public class EmailController : BaseController
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
        if (id != default(Guid))
        {
            var email = await _service.GetEmailByIdAsync(id);
            return View(new EmailForm(email));
        }


        //This should be done elsewhere.
        var emailType = EmailType.TestingDroitsv2;

        try{
            var templateBody = await _service.GetTemplateAsync(emailType);

            return View(new EmailForm()
            {
                Body = templateBody
            });

        }catch(FileNotFoundException e){
            TempData["ErrorMessage"] = "There was an error creating a new email";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    public async Task<IActionResult> SendEmail(Guid id)
    {
        try
        {
            var email = await _service.SendEmailAsync(id);
            if (email != null)
            {
                AddSuccessMessage("Email sent successfully");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Email not found");
            AddErrorMessage("Email not found");
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> SaveEmail(EmailForm form)
    {
        Email email;

        if (!ModelState.IsValid)
        {
            AddErrorMessage("An error occurred while saving the email");
            return View(nameof(Compose), form);
        }

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
}
