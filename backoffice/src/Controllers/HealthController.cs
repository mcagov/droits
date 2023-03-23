using Microsoft.AspNetCore.Mvc;

namespace Droits.Controllers;

public class HealthController : Controller
{
    private readonly ILogger<HealthController> _logger;

    public HealthController(ILogger<HealthController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return new OkResult();
    }
}
