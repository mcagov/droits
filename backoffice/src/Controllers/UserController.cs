using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Models.ViewModels;
using Droits.Models.ViewModels.ListViews;
using Droits.Services;
using Microsoft.AspNetCore.Mvc;

namespace Droits.Controllers;

public class UserController : BaseController
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _service;

    public UserController(ILogger<UserController> logger, IUserService service)
    {
        _logger = logger;
        _service = service;
    }


    public async Task<IActionResult> Index(SearchOptions searchOptions)
    {
        searchOptions.IncludeAssociations = true;
        var model = await _service.GetUserListViewAsync(searchOptions);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> View(Guid id)
    {
        ApplicationUser user;
        try
        {
            user = await _service.GetUserAsync(id);
        }
        catch ( UserNotFoundException e )
        {
            HandleError(_logger, "User not found", e);
            return RedirectToAction(nameof(Index));
        }

        var model = new UserView(user, true);
        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        if ( id == default )
        {
            return View(new UserForm());
        }

        try
        {
            var user = await _service.GetUserAsync(id);
            return View(new UserForm(user));
        }
        catch ( UserNotFoundException e )
        {
            HandleError(_logger, "User not found", e);
            return RedirectToAction(nameof(Index));
        }
    }


    [HttpPost]
    public async Task<IActionResult> Save(UserForm form)
    {
        if ( !ModelState.IsValid )
        {
            AddErrorMessage("Could not save User");
            return View(nameof(Edit), form);
        }

        ApplicationUser user = new ApplicationUser();

        if ( form.Id != default )
        {
            try
            {
                user = await _service.GetUserAsync(form.Id);
            }
            catch ( UserNotFoundException e )
            {
                HandleError(_logger, "User not found", e);
                return View(nameof(Edit), form);
            }
        }

        user = form.ApplyChanges(user);

        try
        {
            await _service.SaveUserAsync(user);
        }
        catch ( Exception e )
        {
            HandleError(_logger, "Unable to save User", e);
            return View(nameof(Edit), form);
        }

        AddSuccessMessage("User saved successfully.");
        return RedirectToAction(nameof(Index));
    }


}
