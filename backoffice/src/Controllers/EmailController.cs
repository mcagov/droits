using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Models.FormModels;
using Droits.Models.ViewModels;
using Droits.Services;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> Edit(Guid id)
    {
        if (id != default)
            try
            {
                var email = await _service.GetEmailByIdAsync(id);
                return View(new EmailForm(email));
            }
            catch (EmailNotFoundException e)
            {
                HandleError(_logger, "Email not found", e);
                return RedirectToAction(nameof(Index));
            }


        //This should be done elsewhere.
        var emailType = EmailType.TestingDroitsv2;

        try
        {
            var templateBody = await _service.GetTemplateAsync(emailType);

            return View(new EmailForm
            {
                Body = templateBody
            });
        }
        catch (FileNotFoundException e)
        {
            HandleError(_logger, "There was an error creating a new Email", e);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    public async Task<IActionResult> SendEmail(Guid id)
    {
        try
        {
            var email = await _service.SendEmailAsync(id);
            if (email != null) AddSuccessMessage("Email sent successfully");
        }
        catch (EmailNotFoundException e)
        {
            HandleError(_logger, "Email not found", e);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> SaveEmail(EmailForm form)
    {
        Email email;

        if (!ModelState.IsValid)
        {
            AddErrorMessage("Could not save Email");
            return View(nameof(Edit), form);
        }

        try
        {
            if (form.EmailId == default)
                email = await _service.SaveEmailAsync(form);
            else
                email = await _service.UpdateEmailAsync(form);
        }
        catch (Exception e)
        {
            HandleError(_logger, "Unable to save email", e);
            return View(nameof(Edit), form);
        }

        return RedirectToAction(nameof(Preview), new { id = email.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Preview(Guid id)
    {
        try
        {
            var email = await _service.GetEmailByIdAsync(id);

            var model = new EmailView(email);

            return View(model);
        }
        catch (EmailNotFoundException e)
        {
            HandleError(_logger, "Email not found", e);
            return RedirectToAction(nameof(Index));
        }
    }
}