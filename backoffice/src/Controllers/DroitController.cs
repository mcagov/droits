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
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(DroitForm form)
    {
        var droit = form.ApplyChanges(new Droit());

        await _service.AddDroitAsync(droit);
        return RedirectToAction(nameof(Index));
    }
}