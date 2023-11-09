using Droits.Models.ViewModels;
using Droits.Models.ViewModels.ListViews;
using Droits.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Droits.Controllers;

public class AccountController : BaseController
{
    private readonly ILogger<AccountController> _logger;
    private readonly IDroitService _droitService;
    private readonly IAccountService _accountService;
    private readonly ILetterService _letterService;

    public AccountController(ILogger<AccountController> logger, IDroitService droitService, IAccountService accountService, ILetterService letterService)
    {
        _logger = logger;
        _droitService = droitService;
        _accountService = accountService;
        _letterService = letterService;
    }

    
    public async Task<IActionResult> Index(SearchOptions searchOptions)
    {
        searchOptions.IncludeAssociations = true;
        searchOptions.FilterByAssignedUser = true;
        
        var droits = await _droitService.GetDroitsListViewAsync(searchOptions);
        var letters = await _letterService.GetApprovedUnsentLettersListViewForCurrentUserAsync(searchOptions);
        return View(new DashboardView(droits,letters));
    }

    public IActionResult Info()
    {
        return View();
    }

    public IActionResult Profile()
    {
        var id = _accountService.GetCurrentUserId();
        return RedirectToAction("View", "User", new{id});
    }
    
    [AllowAnonymous]
    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Index","Home");
    }
}
