using Microsoft.AspNetCore.Mvc;

namespace Droits.Controllers;

public class BaseController : Controller
{
    protected void AddSuccessMessage(string message)
    {
        TempData["SuccessMessage"] = message;
    }
    
    protected void AddErrorMessage(string message)
    {
        TempData["ErrorMessage"] = message;
    }
}