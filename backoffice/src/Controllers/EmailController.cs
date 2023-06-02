using Microsoft.AspNetCore.Mvc;
using Droits.Models.Email;
using Droits.Services;
using Notify.Models.Responses;


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
        EmailForm model = _service.GetEmailForm("Views/Email/templates", EmailTemplateType.TestingDroitsv2);

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> SendEmail(EmailForm form)
    {
        var result = await _service.SendEmailAsync(form);

        return View(nameof(SendEmail), form);
    }

    [HttpPost]
    public IActionResult GetPreview(EmailForm form)
    {
        var preview = _service.GetPreview(form);

        return View(nameof(GetPreview), preview);
    }
}