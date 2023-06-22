using Droits.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Droits.Models;
using Droits.Services;

namespace Droits.Controllers;

public class WreckController : BaseController
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
        var wreck = new Wreck();
        try
        {
            wreck = await _service.GetWreckAsync(id);
        }
        catch (WreckNotFoundException e)
        {
            //Add Logger?
            AddErrorMessage("Wreck not found");
            return RedirectToAction(nameof(Index));
        }

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
        if (id == default(Guid))
        {
            return View(new WreckForm());
        }

        try
        {
            var wreck = await _service.GetWreckAsync(id);

            var model = new WreckForm(wreck);

            return View(model);
        }
        catch (WreckNotFoundException e)
        {
            AddErrorMessage("Droit not found");
            return RedirectToAction(nameof(Index));   
        }
    }


    [HttpPost]
    public async Task<IActionResult> Save(WreckForm form)
    {
        var wreck = new Wreck();

        if(form.Id != default(Guid)){
            wreck = await _service.GetWreckAsync(form.Id);
            if (wreck == null)
            {
                AddErrorMessage("Wreck not found");
            };
        }

        wreck = form.ApplyChanges(wreck);

        await _service.SaveWreckAsync(wreck);
        return RedirectToAction(nameof(Index));
    }
}
