using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Droits.Services;

namespace Droits.Controllers;

public class SalvorController : BaseController
{
    private readonly ILogger<SalvorController> _logger;
    private readonly ISalvorService _service;


    public SalvorController(ILogger<SalvorController> logger, ISalvorService service)
    {
        _logger = logger;
        _service = service;
    }


    public async Task<IActionResult> Index()
    {
        var salvors = await _service.GetSalvorsAsync();

        var model = salvors.Select(s => new SalvorView(s)).ToList();

        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> View(Guid id)
    {
        Salvor salvor;
        try
        {
            salvor = await _service.GetSalvorAsync(id);
        }
        catch ( SalvorNotFoundException e )
        {
            HandleError(_logger, "Salvor not found", e);
            return RedirectToAction(nameof(Index));
        }

        var model = new SalvorView(salvor, true);
        return View(model);
    }


    [HttpGet]
    public IActionResult Add()
    {
        var model = new SalvorForm();
        return View(nameof(Edit), model);
    }


    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        if ( id == default )
        {
            return View(new SalvorForm());
        }

        try
        {
            var salvor = await _service.GetSalvorAsync(id);
            return View(new SalvorForm(salvor));
        }
        catch ( SalvorNotFoundException e )
        {
            HandleError(_logger, "Salvor not found", e);
            return RedirectToAction(nameof(Index));
        }
    }


    [HttpPost]
    public async Task<IActionResult> Save(SalvorForm form)
    {
        if ( !ModelState.IsValid )
        {
            AddErrorMessage("Could not save Salvor");
            return View(nameof(Edit), form);
        }

        var salvor = new Salvor();

        if ( form.Id != default )
        {
            try
            {
                salvor = await _service.GetSalvorAsync(form.Id);
            }
            catch ( SalvorNotFoundException e )
            {
                HandleError(_logger, "Salvor not found", e);
                return View(nameof(Edit), form);
            }
        }

        salvor = form.ApplyChanges(salvor);

        try
        {
            await _service.SaveSalvorAsync(salvor);
        }
        catch ( SalvorNotFoundException e )
        {
            HandleError(_logger, "Unable to save Salvor", e);
            return View(nameof(Edit), form);
        }

        AddSuccessMessage("Salvor saved successfully.");
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    public async Task<ActionResult> SalvorViewPartial(Guid id)
    {
        try
        {
            var salvor = await _service.GetSalvorAsync(id);
            return PartialView("Salvor/_SalvorViewFields", new SalvorView(salvor));
        }
        catch ( SalvorNotFoundException e )
        {
            _logger.LogError("Salvor not found for partial", e);
            return NotFound();
        }
    }
}