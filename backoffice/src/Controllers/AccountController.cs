using Droits.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Droits.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace Droits.Controllers;

public class AccountController : BaseController
{
    private readonly ILogger<AccountController> _logger;
    private readonly IDroitService _droitService;
    private readonly IAccountService _accountService;

    public AccountController(ILogger<AccountController> logger, IDroitService droitService, IAccountService accountService)
    {
        _logger = logger;
        _droitService = droitService;
        _accountService = accountService;
    }

    
    public async Task<IActionResult> Index(SearchOptions searchOptions)
    {
        searchOptions.IncludeAssociations = true;
        searchOptions.FilterByAssignedUser = true;
        
        var model = await _droitService.GetDroitsListViewAsync(searchOptions);
        return View(model);
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
