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
    public async Task<IActionResult> View(Guid id, string? objectReference)
    {
        try
        {
            var note = await _service.GetNoteAsync(id);
            var viewModel = new NoteView(note);
            if ( !string.IsNullOrEmpty(objectReference) )
            {
                viewModel.ObjectReference = objectReference;
            }
            
            return View(viewModel);
        }
        catch ( NoteNotFoundException e )
        {
            HandleError(_logger, "Note not found", e);
            return RedirectToAction(nameof(Index));
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var note = await _service.GetNoteAsync(id);
            var (controllerName, entityId) = new NoteView(note).GetAssociatedEntityInfo();
            
            var result = await _service.DeleteNoteAsync(note.Id);

            if ( !result )
            {
                AddErrorMessage("Note could not be deleted");
                return RedirectToAction("View", new { id = note.Id });
            }
            
            AddSuccessMessage("Note has ben successfully deleted.");
            if (!string.IsNullOrEmpty(controllerName) && entityId.HasValue)
            {
                return RedirectToAction("View", controllerName, new { id = entityId.Value });
            }
            
            return RedirectToAction(nameof(Index));
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
    public async Task<IActionResult> Edit(Guid id, string? objectReference)
    {

        var noteForm = new NoteForm();
        if ( id != default )
        {
            try
            {
                var note = await _service.GetNoteAsync(id);
                noteForm = new NoteForm(note);
                if ( !string.IsNullOrEmpty(objectReference) )
                {
                    noteForm.ObjectReference = objectReference;
                }
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

        
        return RedirectToAction("View", new { id = note.Id });
        
    }

}
