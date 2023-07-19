using Droits.Exceptions;
using Droits.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Droits.Services;
using Droits.Models.ViewModels;
using Droits.Models.FormModels;
using Droits.Models.Enums;

namespace Droits.Controllers;

public class LetterController : BaseController
{
    private readonly ILogger<LetterController> _logger;
    private readonly ILetterService _service;


    public LetterController(ILogger<LetterController> logger, ILetterService service)
    {
        _logger = logger;
        _service = service;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var letters = await _service.GetLettersAsync();

        var letterViews = letters.Select(e => new LetterView(e)).ToList();

        var model = new LetterListView(letterViews);
        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        if ( id != default )
        {
            try
            {
                var letter = await _service.GetLetterByIdAsync(id);
                return View(new LetterForm(letter));
            }
            catch ( LetterNotFoundException e )
            {
                HandleError(_logger, "Letter not found", e);
                return RedirectToAction(nameof(Index));
            }
        }


        //This should be done elsewhere.
        var letterType = LetterType.TestingDroitsv2;

        try
        {
            var templateBody = await _service.GetTemplateAsync(letterType);

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


    [HttpGet]
    public async Task<IActionResult> SendLetter(Guid id)
    {
        try
        {
            var letter = await _service.SendLetterAsync(id);
            if ( letter != null )
            {
                AddSuccessMessage("Letter sent successfully");
            }
        }
        catch ( LetterNotFoundException e )
        {
            HandleError(_logger, "Letter not found", e);
        }

        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    public async Task<IActionResult> SaveLetter(LetterForm form)
    {
        Letter letter;

        if ( !ModelState.IsValid )
        {
            AddErrorMessage("Could not save Letter");
            return View(nameof(Edit), form);
        }

        try
        {
            if ( form.LetterId == default )
            {
                letter = await _service.SaveLetterAsync(form);
            }
            else
            {
                letter = await _service.UpdateLetterAsync(form);
            }
        }
        catch ( Exception e )
        {
            HandleError(_logger, "Unable to save letter", e);
            return View(nameof(Edit), form);
        }

        return RedirectToAction(nameof(Preview), new { id = letter.Id });
    }


    [HttpGet]
    public async Task<IActionResult> Preview(Guid id)
    {
        try
        {
            var letter = await _service.GetLetterByIdAsync(id);

            var model = new LetterView(letter);

            return View(model);
        }
        catch ( LetterNotFoundException e )
        {
            HandleError(_logger, "Letter not found", e);
            return RedirectToAction(nameof(Index));
        }
    }
}