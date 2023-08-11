using Droits.Exceptions;
using Droits.Helpers.Extensions;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Models.FormModels;
using Droits.Models.ViewModels;
using Droits.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Droits.Controllers;

public class DroitController : BaseController
{
    private readonly ILogger<DroitController> _logger;
    private readonly IDroitService _service;
    private readonly IWreckService _wreckService;
    private readonly ISalvorService _salvorService;
    private readonly IUserService _userService;


    public DroitController(ILogger<DroitController> logger, IDroitService service,
        IWreckService wreckService,
        ISalvorService salvorService,
        IUserService userService)
    {
        _logger = logger;
        _service = service;
        _wreckService = wreckService;
        _salvorService = salvorService;
        _userService = userService;
    }


    public async Task<IActionResult> Index(SearchOptions searchOptions)
    {
        searchOptions.IncludeAssociations = true;
        var model = await _service.GetDroitsListViewAsync(searchOptions);
        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> View(Guid id)
    {
        Droit droit;
        try
        {
            droit = await _service.GetDroitWithAssociationsAsync(id);
        }
        catch ( DroitNotFoundException e )
        {
            HandleError(_logger, "Droit not found.", e);
            return RedirectToAction(nameof(Index));
        }

        var model = new DroitView(droit);
        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> Add()
    {
        var form = await PopulateDroitFormAsync(new DroitForm());
        return View(nameof(Edit), form);
    }


    [HttpPost]
    public async Task<IActionResult> UpdateDroitStatus(Guid id, DroitStatus status)
    {
        if ( !ModelState.IsValid )
        {
            AddErrorMessage("Invalid Status");
            return RedirectToAction(nameof(View), id);
        }

        await _service.UpdateDroitStatusAsync(id, status);

        return RedirectToAction(nameof(View), new { id = id });
    }


    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        if ( id == default )
        {
            var form = await PopulateDroitFormAsync(new DroitForm());
            return View(form);
        }

        try
        {
            var droit = await _service.GetDroitAsync(id);
            var form = await PopulateDroitFormAsync(new DroitForm(droit));
            return View(form);
        }
        catch ( DroitNotFoundException e )
        {
            HandleError(_logger, "Droit not found.", e);
            return RedirectToAction(nameof(Index));
        }
    }


    [HttpPost]
    public async Task<IActionResult> Save(DroitForm form)
    {
        if ( form.WreckId.HasValue || form.IsIsolatedFind )
        {
            ModelState.RemoveStartingWith("WreckForm");
        }

        if ( form.SalvorId.HasValue )
        {
            ModelState.RemoveStartingWith("SalvorForm");
        }

        form.WreckMaterialForms
            .Select((wmForm, i) => new { Form = wmForm, Index = i })
            .Where(item => item.Form.StoredAtSalvor)
            .ToList()
            .ForEach(item =>
                ModelState.RemoveStartingWith($"WreckMaterialForms[{item.Index}].StorageAddress"));

        if ( !ModelState.IsValid )
        {
            AddErrorMessage("Could not save Droit");
            form = await PopulateDroitFormAsync(form);
            return View(nameof(Edit), form);
        }

        var droit = new Droit();

        if ( form.Id != default )
        {
            try
            {
                droit = await _service.GetDroitAsync(form.Id);
            }
            catch ( DroitNotFoundException e )
            {
                HandleError(_logger, "Droit not found.", e);
                return View(nameof(Edit), form);
            }
        }

        droit = form.ApplyChanges(droit);

        if ( !droit.WreckId.HasValue && !form.IsIsolatedFind )
        {
            droit.WreckId = await _wreckService.SaveWreckFormAsync(form.WreckForm);
        }

        if ( !droit.SalvorId.HasValue )
        {
            droit.SalvorId = await _salvorService.SaveSalvorFormAsync(form.SalvorForm);
        }

        try
        {
            droit = await _service.SaveDroitAsync(droit);
        }
        catch ( Exception e )
        {
            HandleError(_logger, "Could not save Droit.", e);
            form = await PopulateDroitFormAsync(form);
            return View(nameof(Edit), form);
        }


        try
        {
            await _service.SaveWreckMaterialsAsync(droit.Id, form.WreckMaterialForms);
        }
        catch ( Exception e )
        {
            HandleError(_logger, "Could not save Wreck Material.", e);
            form = await PopulateDroitFormAsync(form);
            return View(nameof(Edit), form);
        }

        AddSuccessMessage("Droit saved successfully");

        return RedirectToAction(nameof(Index));
    }


    [HttpGet]
    public ActionResult WreckMaterialFormPartial()
    {
        return PartialView("WreckMaterial/_WreckMaterialFormFields", new WreckMaterialForm());
    }


    private async Task<DroitForm> PopulateDroitFormAsync(DroitForm form)
    {
        var allUsers = await _userService.GetUsersAsync();
        var allWrecks = await _wreckService.GetWrecksAsync();
        var allSalvors = await _salvorService.GetSalvorsAsync();

        form.AllUsers = allUsers
            .Select(u => new SelectListItem($"{u.Name} ({u.Email})", u.Id.ToString())).ToList();
        form.AllWrecks =
            allWrecks.Select(w => new SelectListItem(w.Name, w.Id.ToString())).ToList();
        form.AllSalvors = allSalvors
            .Select(s => new SelectListItem($"{s.Name} ({s.Email})", s.Id.ToString())).ToList();

        return form;
    }
}