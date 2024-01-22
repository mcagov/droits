#region

using Droits.Exceptions;
using Droits.Helpers.Extensions;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Models.ViewModels;
using Droits.Services;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace Droits.Controllers;

public class NoteController : BaseController
{
    private readonly ILogger<NoteController> _logger;
    private readonly INoteService _service;

    public NoteController(ILogger<NoteController> logger, INoteService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet]
    public IActionResult Index()
    {
        //For the Edit screen to work, but never used.
        throw new NotImplementedException();
    }
    
    
    [HttpGet]
    public async Task<IActionResult> View(Guid id)
    {
        try
        {
            var note = await _service.GetNoteAsync(id);
            return View(new NoteView(note));
        }
        catch ( NoteNotFoundException e )
        {
            HandleError(_logger, "Note not found", e);
            return RedirectToAction(nameof(Index));
        }
    }
    
    [HttpGet]
    public IActionResult Add(NoteForm noteForm)
    {
        ModelState.Remove("Title");
        ModelState.Remove("Text");
        return View("Edit", noteForm);
    }

    
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {

        var noteForm = new NoteForm();
        if ( id != default )
        {
            try
            {
                var note = await _service.GetNoteAsync(id);
                noteForm = new NoteForm(note);
            }
            catch ( NoteNotFoundException e )
            {
                HandleError(_logger, "Note not found", e);
                return RedirectToAction(nameof(Index));
            }
        }
        
        
        return View(noteForm);
    }

    [HttpPost]
    public async Task<IActionResult> Save(NoteForm form)
    {

       ModelState.RemoveStartingWith("DroitFileForms");

       if (!ModelState.IsValid)
       {
            AddErrorMessage("Could not save Note");
            return View(nameof(Edit), form);
       }

        var note = new Note();

        if (form.Id != default)
        {
            try
            {
                note = await _service.GetNoteAsync(form.Id);
            }
            catch (NoteNotFoundException e)
            {
                HandleError(_logger, "Note not found", e);
                return View(nameof(Edit), form);
            }
        }

        note = form.ApplyChanges(note);

        try
        { 
            note = await _service.SaveNoteAsync(note);
            await _service.SaveFilesAsync(note.Id, form.DroitFileForms);
        }
        catch (Exception e)
        {
            HandleError(_logger, "Unable to save note", e);
            return View(nameof(Edit), form);
        }

        AddSuccessMessage("Note saved successfully.");

        var (controllerName, entityId) = form.GetAssociatedEntityInfo();

        if (!string.IsNullOrEmpty(controllerName) && entityId.HasValue)
        {
            return RedirectToAction("View", controllerName, new { id = entityId.Value });
        }

        AddErrorMessage("No associated entity found for the note.");
        return View(nameof(Edit), form);
    }

}
