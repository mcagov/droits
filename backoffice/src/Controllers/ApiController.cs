using Microsoft.AspNetCore.Mvc;
using Droits.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Droits.Controllers;

public class ApiController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public ApiController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(){
     return Json(new {status = "Got here..."});
    }

    [HttpPost]
    public async Task<IActionResult> Send()
    {
        using (var reader = new StreamReader(Request.Body))
        {
            var body = await reader.ReadToEndAsync();
            WreckReport report = JsonConvert.DeserializeObject<WreckReport>(body);

            Console.WriteLine(JValue.Parse(body).ToString(Formatting.Indented));

            return Json(report);
        }

    }


}
