using Droits.Exceptions;
using Droits.Models.Entities;
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

    public DroitController(ILogger<DroitController> logger, IDroitService service, IWreckService wreckService)
    {
        _logger = logger;
        _service = service;
        _wreckService = wreckService;
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
            HandleError(_logger,"Droit not found.",e);
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

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        if (id == default(Guid))
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
        catch (DroitNotFoundException e)
        {
            HandleError(_logger,"Droit not found.",e);
            return RedirectToAction(nameof(Index));
        }
    }


    [HttpPost]
    public async Task<IActionResult> Save(DroitForm form)
    {

        if(form.WreckId.HasValue)
        {
            ModelState.Remove("WreckForm.Name");
        }

        if (!ModelState.IsValid)
        {
            AddErrorMessage("Could not save Droit");
            form = await PopulateDroitFormAsync(form);
            return View(nameof(Edit), form);
        }

        var droit = new Droit();

        if(form.Id != default(Guid)){

            try{
                droit = await _service.GetDroitAsync(form.Id);
            }catch(DroitNotFoundException e){
                HandleError(_logger,"Droit not found.",e);
                return View(nameof(Edit), form);
            }
        }

        droit = form.ApplyChanges(droit);

        if(!droit.WreckId.HasValue){
            droit.WreckId = await _wreckService.SaveWreckFormAsync(form.WreckForm);
        }

        try{
            droit = await _service.SaveDroitAsync(droit);
        }catch(Exception e){
            HandleError(_logger,"Could not save Droit.",e);
            form = await PopulateDroitFormAsync(form);
            return View(nameof(Edit), form);
        }


        try{
            await _service.SaveWreckMaterialsAsync(droit.Id, form.WreckMaterialForms);
        }catch(Exception e){
            HandleError(_logger,"Could not save Wreck Material.",e);
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

    private async Task<DroitForm> PopulateDroitFormAsync(DroitForm form){

        var allWrecks = await _wreckService.GetWrecksAsync();

        form.AllWrecks =  allWrecks.Select(w => new SelectListItem(w.Name, w.Id.ToString())).ToList();

        return form;
    }

}
