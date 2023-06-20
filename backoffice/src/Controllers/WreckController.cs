using Microsoft.AspNetCore.Mvc;
using Droits.Models;
using Droits.Services;

namespace Droits.Controllers;

public class WreckController : Controller
{
    private readonly ILogger<WreckController> _logger;
    private readonly IWreckService _service;

    public WreckController(ILogger<WreckController> logger, IWreckService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var wrecks = await _service.GetWrecksAsync();

        var model = wrecks.Select(w => new WreckView(w)).ToList();

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> View(Guid id)
    {
        var wreck = await _service.GetWreckAsync(id);

        var model = new WreckView(wreck);
        return View(model);
    }

    [HttpGet]
    public IActionResult Add()
    {
        var model = new WreckForm();
        return View(nameof(Edit),model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var wreck = await _service.GetWreckAsync(id);

        var model = new WreckForm(wreck);

        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> Save(WreckForm form)
    {
        var wreck = new Wreck();

        if(form.Id != default(Guid)){
            wreck = await _service.GetWreckAsync(form.Id);
            if(wreck == null) return NotFound();
        }

        wreck = form.ApplyChanges(wreck);

        await _service.SaveWreckAsync(wreck);
        return RedirectToAction(nameof(Index));
    }
}
