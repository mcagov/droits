#region

using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Models.FormModels;
using Droits.Models.FormModels.SearchFormModels;
using Droits.Models.ViewModels;
using Droits.Models.ViewModels.ListViews;
using Droits.Services;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace Droits.Controllers;

public class LetterController : BaseController
{
    private readonly ILogger<LetterController> _logger;
    private readonly ILetterService _service;
    private readonly IDroitService _droitService;


    public LetterController(ILogger<LetterController> logger, ILetterService service,
        IDroitService droitService)
    {
        _logger = logger;
        _service = service;
        _droitService = droitService;
    }


    [HttpGet]
    public async Task<IActionResult> Index(SearchOptions searchOptions)
    {
        searchOptions.IncludeAssociations = true;
        var model = await _service.GetLettersListViewAsync(searchOptions);
        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        if ( id != default )
        {
            try
            {
                var letter = await _service.GetLetterAsync(id);
                return View(new LetterForm(letter));
            }
            catch ( LetterNotFoundException e )
            {
                HandleError(_logger, "Letter not found", e);
                return RedirectToAction(nameof(Index));
            }
        }


        //This should be done elsewhere.
        var letterType = LetterType.CustomLetter;

        try
        {
            var templateBody = await _service.GetTemplateBodyAsync(letterType, null);

            return View(new LetterForm()
            {
                Body = templateBody
            });
        }
        catch ( FileNotFoundException e )
        {
            HandleError(_logger, "There was an error creating a new Letter", e);
            return RedirectToAction(nameof(Index));
        }
    }


    [HttpPost]
    public async Task<IActionResult> AddLetterToDroit(Guid droitId, LetterType type)
    {
        var droit = new Droit();
        try
        {
            droit = await _droitService.GetDroitWithAssociationsAsync(droitId);
        }
        catch ( DroitNotFoundException e )
        {
            HandleError(_logger, "Droit not found.", e);
            return RedirectToAction("Index", "Droit");
        }

        var model = new LetterForm
        {
            DroitId = droit.Id,
            DroitReference = droit.Reference,
            Recipient = droit?.Salvor?.Email ?? "",
            Type = type
        };


        model.Subject = await _service.GetTemplateSubjectAsync(type, droit);
        model.Body = await _service.GetTemplateBodyAsync(type, droit);
        return View(nameof(Edit), model);
    }


    [HttpGet]
    public async Task<IActionResult> SendLetter(Guid id)
    {
        try
        {
            var letter = await _service.SendLetterAsync(id);

            AddSuccessMessage("Letter sent successfully");
            if ( letter.DroitId != default )
            {
                return RedirectToAction("View", "Droit", new { id = letter.DroitId });
            }
        }
        catch ( LetterNotFoundException e )
        {
            HandleError(_logger, "Letter not found", e);
        }

        
        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    public async Task<IActionResult> Save(LetterForm form)
    {
        Letter letter;

        if ( !ModelState.IsValid )
        {
            AddErrorMessage("Could not save Letter");
            return View(nameof(Edit), form);
        }

        try
        {
            letter = await _service.SaveLetterAsync(form);
        }
        catch ( Exception e )
        {
            HandleError(_logger, "Unable to save letter", e);
            return View(nameof(Edit), form);
        }

        return RedirectToAction(nameof(View), new { id = letter.Id });
    }


    [HttpGet]
    public async Task<IActionResult> View(Guid id)
    {
        try
        {
            var letter = await _service.GetLetterAsync(id);

            var model = new LetterView(letter, true);

            return View(model);
        }
        catch ( LetterNotFoundException e )
        {
            HandleError(_logger, "Letter not found", e);
            return RedirectToAction(nameof(Index));
        }
    }
    
    
    public async Task<IActionResult> Search(LetterSearchForm form)
    {
        if (form.SubmitAction != "Search")
        {
            switch ( form.SubmitAction )
            {
                case "Export" : 
                    try
                    {
                        var csvExport = await _service.ExportAsync(form); 
                        return File(csvExport, "text/csv", $"letters-export-{DateTime.UtcNow.ToShortDateString()}.csv");
                    }
                    catch ( Exception e )
                    {
                        HandleError(_logger, "No Salvors to export", e);
                        return RedirectToAction("Index");
                    }

                default:
                    return RedirectToAction(form.SubmitAction, form);
            }
        }

        form.IncludeAssociations = true;
        
        var model = await _service.AdvancedSearchAsync(form);
        
        model.SearchOpen = model.PageNumber == 1;

        return View(nameof(Index), model);
    }
    
}
