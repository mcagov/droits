using Droits.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Droits.Models;
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

        var model = salvors.Select(w => new SalvorView(w)).ToList();

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> View(Guid id)
    {
        var salvor = new Salvor();
        try
        {
            salvor = await _service.GetSalvorAsync(id);
        }
        catch (SalvorNotFoundException e)
        {
            HandleError(_logger,"Salvor not found",e);
            return RedirectToAction(nameof(Index));
        }

        var model = new SalvorView(salvor);
        return View(model);
    }

    [HttpGet]
    public IActionResult Add()
    {
        var model = new SalvorForm();
        return View(nameof(Edit),model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        if (id == default(Guid))
        {
            return View(new SalvorForm());
        }

        try
        {
            var salvor = await _service.GetSalvorAsync(id);
            return View(new SalvorForm(salvor));
        }
        catch (SalvorNotFoundException e)
        {
            HandleError(_logger,"Salvor not found",e);
            return RedirectToAction(nameof(Index));
        }
    }


    [HttpPost]
    public async Task<IActionResult> Save(SalvorForm form)
    {

        if (!ModelState.IsValid)
        {
            AddErrorMessage("Could not save Salvor");
            return View(nameof(Edit), form);
        }

        var salvor = new Salvor();

        if(form.Id != default(Guid)){
            try{
                salvor = await _service.GetSalvorAsync(form.Id);
            }
            catch(SalvorNotFoundException e)
            {
                HandleError(_logger,"Salvor not found",e);
                return View(nameof(Edit), form);
            }
        }

        salvor = form.ApplyChanges(salvor);

        try{
            await _service.SaveSalvorAsync(salvor);
        }
        catch(SalvorNotFoundException e)
        {
            HandleError(_logger,"Unable to save Salvor",e);
            return View(nameof(Edit), form);
        }

        AddSuccessMessage("Salvor saved successfully.");
        return RedirectToAction(nameof(Index));
    }
}
