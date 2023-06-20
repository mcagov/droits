using Droits.Models;
using Droits.Services;
using Microsoft.AspNetCore.Mvc;

namespace Droits.Controllers;

public class DroitController : Controller
{
    private readonly ILogger<DroitController> _logger;
    private readonly IDroitService _service;

    public DroitController(ILogger<DroitController> logger, IDroitService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var droits = await _service.GetDroitsAsync();

        var model = droits.Select(d => new DroitView(d)).ToList();

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> View(Guid id)
    {
        var droit = await _service.GetDroitAsync(id);

        if (droit == null) return NotFound();

        var model = new DroitView(droit);
        return View(model);
    }

    [HttpGet]
    public IActionResult Add()
    {
        var model = new DroitForm();
        return View(nameof(Edit),model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var droit = await _service.GetDroitAsync(id);

        if(droit == null) return NotFound();

        var model = new DroitForm(droit);

        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> Save(DroitForm form)
    {
        var droit = new Droit();

        if(form.Id != default(Guid)){
            droit = await _service.GetDroitAsync(form.Id);
            if(droit == null) return NotFound();
        }

        droit = form.ApplyChanges(droit);

        await _service.SaveDroitAsync(droit);
        return RedirectToAction(nameof(Index));
    }
}
