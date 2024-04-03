#region

using System.Globalization;
using Droits.Helpers.Extensions;
using Droits.Models.Enums;
using Droits.Models.FormModels.SearchFormModels;
using Droits.Models.ViewModels;
using Droits.Models.ViewModels.ListViews;
using Droits.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#endregion

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

    
    public async Task<IActionResult> Index(DashboardView model)
    {
        var searchOptions = model.DashboardSearchForm;
        
        searchOptions.IncludeAssociations = true;
        searchOptions.FilterByAssignedUser = true;


        searchOptions.PageNumber = searchOptions.DroitsPageNumber;
        var droits = await _droitService.GetDroitsListViewAsync(searchOptions);
        
        searchOptions.PageNumber = searchOptions.LettersPageNumber;
        var letters = await _letterService.GetApprovedUnsentLettersListViewForCurrentUserAsync(searchOptions);
        
        return View(new DashboardView(droits,letters));
    }
    
    public IActionResult MetricsDashboard()
    {
        return View();
    }
 
    
    public async Task<IActionResult> MetricsData()
    {
        var droitMetrics = await _droitService.GetDroitsMetrics();
        return Json(droitMetrics);
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
