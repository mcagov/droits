using Microsoft.AspNetCore.Mvc;
using Droits.Models;
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
        return View(new EmailForm());
    }

    [HttpPost]
    public async Task<IActionResult> SendEmail(EmailForm form)
    {
        var result = await _service.SendEmailAsync(form);

        return View(nameof(EmailController.SendEmail), form);
    }


}
