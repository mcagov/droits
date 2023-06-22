using System.Linq.Expressions;
using Droits.Exceptions;
using Droits.Models;
using Droits.Services;
using Microsoft.AspNetCore.Mvc;

namespace Droits.Controllers;

public class DroitController : BaseController
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
        var droit = new Droit();
        try
        {
            droit = await _service.GetDroitAsync(id);
        }
        catch (DroitNotFoundException e)
        {
            //Add Logger?
            AddErrorMessage("Droit not found");
            return RedirectToAction(nameof(Index));
        }

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
        if (id == default(Guid))
        {
            return View(new DroitForm());
        }
        
        try
        {
          var droit = await _service.GetDroitAsync(id);
          return View(new DroitForm(droit));
        }
        catch (DroitNotFoundException e) 
        { 
            AddErrorMessage("Droit not found");
            return RedirectToAction(nameof(Index),id);    
        }
    }


    [HttpPost]
    public async Task<IActionResult> Save(DroitForm form)
    {
        //To-Do refactor this
        var droit = new Droit();

        if(form.Id != default(Guid)){
            droit = await _service.GetDroitAsync(form.Id);
            if (droit == null)
            {
                AddErrorMessage("Droit not found");
            }
        }

        droit = form.ApplyChanges(droit);

        await _service.SaveDroitAsync(droit);
        return RedirectToAction(nameof(Index));
    }
}
