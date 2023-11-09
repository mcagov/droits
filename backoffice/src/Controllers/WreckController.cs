using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Models.FormModels.SearchFormModels;
using Droits.Models.ViewModels;
using Droits.Models.ViewModels.ListViews;
using Droits.Services;
using Microsoft.AspNetCore.Mvc;

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


    public async Task<IActionResult> Index(SearchOptions searchOptions)
    {
        searchOptions.IncludeAssociations = true;
        var model = await _service.GetWrecksListViewAsync(searchOptions);
        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> View(Guid id)
    {
        Wreck wreck;
        try
        {
            wreck = await _service.GetWreckAsync(id);
        }
        catch ( WreckNotFoundException e )
        {
            HandleError(_logger, "Wreck not found", e);
            return RedirectToAction(nameof(Index));
        }

        var model = new WreckView(wreck, true);
        return View(model);
    }


    [HttpGet]
    public IActionResult Add()
    {
        var model = new WreckForm();
        return View(nameof(Edit), model);
    }


    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        if ( id == default )
        {
            return View(new WreckForm());
        }

        try
        {
            var wreck = await _service.GetWreckAsync(id);
            return View(new WreckForm(wreck));
        }
        catch ( WreckNotFoundException e )
        {
            HandleError(_logger, "Wreck not found", e);
            return RedirectToAction(nameof(Index));
        }
    }


    [HttpPost]
    public async Task<IActionResult> Save(WreckForm form)
    {
        if ( !ModelState.IsValid )
        {
            AddErrorMessage("Could not save Wreck");
            return View(nameof(Edit), form);
        }

        var wreck = new Wreck();

        if ( form.Id != default )
        {
            try
            {
                wreck = await _service.GetWreckAsync(form.Id);
            }
            catch ( WreckNotFoundException e )
            {
                HandleError(_logger, "Wreck not found", e);
                return View(nameof(Edit), form);
            }
        }

        wreck = form.ApplyChanges(wreck);

        try
        {
            await _service.SaveWreckAsync(wreck);
        }
        catch ( WreckNotFoundException e )
        {
            HandleError(_logger, "Unable to save Wreck", e);
            return View(nameof(Edit), form);
        }

        AddSuccessMessage("Wreck saved successfully.");
        return wreck.Id == default ? RedirectToAction(nameof(Index)) : RedirectToAction(nameof(View),new {id = wreck.Id});
    }


    [HttpGet]
    public async Task<ActionResult> WreckViewPartial(Guid id)
    {
        try
        {
            var wreck = await _service.GetWreckAsync(id);
            return PartialView("Wreck/_WreckViewFields", new WreckView(wreck));
        }
        catch ( WreckNotFoundException e )
        {
            _logger.LogError("Wreck not found for partial", e);
            return NotFound();
        }
    }
    
    public async Task<IActionResult> Search(WreckSearchForm form)
    {
        if (form.SubmitAction != "Search")
        {
                return RedirectToAction(form.SubmitAction,form);
        }
        
        form.IncludeAssociations = true;
        
        var model = await _service.AdvancedSearchAsync(form);
        
        model.SearchOpen = true;
        
        return View(nameof(Index), model);
    }
    
    public async Task<IActionResult> Export(WreckSearchForm form)
    {
        var csvExport = await _service.ExportAsync(form);

        return File(csvExport, "text/csv", $"wreck-export-{DateTime.UtcNow.ToShortDateString()}.csv");
    }
}