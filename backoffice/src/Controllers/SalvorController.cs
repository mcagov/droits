using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Models.FormModels.SearchFormModels;
using Droits.Models.ViewModels;
using Droits.Models.ViewModels.ListViews;
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


    public async Task<IActionResult> Index(SearchOptions searchOptions)
    {
        searchOptions.IncludeAssociations = true;
        var model = await _service.GetSalvorListViewAsync(searchOptions);
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
        return salvor.Id == default ? RedirectToAction(nameof(Index)) : RedirectToAction(nameof(View),new {id = salvor.Id});
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


    public async Task<IActionResult> Search(SalvorSearchForm form, string command)
    {
        switch (command)
        {
            case "exportOne":
                return RedirectToAction("Export",form);
            case "exportTwo":
                return RedirectToAction("ExportTwo",form);
        }
        
        form.IncludeAssociations = true;
                    
        var model = await _service.AdvancedSearchAsync(form);
                    
        return View(nameof(Index), model);
        
    }


    public async Task<IActionResult> Export(SalvorSearchForm form)
    {
        
        form.PageSize = Int32.MaxValue;
        var salvorViews = await _service.AdvancedSearchAsync(form);
        
        var csvExport = await _service.ExportSalvorsAsync(salvorViews);

        return File(csvExport, "text/csv", $"salvor-export-{DateTime.UtcNow.ToShortDateString()}.csv");
    }
    
    public async Task<IActionResult> ExportTwo(SalvorSearchForm form)
    {
        
        var query = _service.QueryFromForm(form);

        var salvors = _service.SearchSalvors(query);
        
        var csvExport = await _service.ExportSalvorsTwoAsync(salvors);

        return File(csvExport, "text/csv", $"salvor-export-{DateTime.UtcNow.ToShortDateString()}.csv");
    }
}