using Microsoft.AspNetCore.Mvc;
using Droits.Models;
using Droits.Services;
using System.Text;

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

        var template = "";
        using (StreamReader streamReader = new StreamReader("Views/Email/templates/acknowledged.txt", Encoding.UTF8))
        {
            template = streamReader.ReadToEnd();
        }


        var model = new EmailForm(){
            Body = template
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> SendEmail(EmailForm form)
    {
        var result = await _service.SendEmailAsync(form);

        return View(nameof(EmailController.SendEmail), form);
    }


}
