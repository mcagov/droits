#region

using Droits.Helpers.Extensions;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace Droits.Controllers;

public class BaseController : Controller
{
    protected void AddSuccessMessage(string message)
    {
        TempData.SetSuccessMessage(message);
    }


    protected void AddErrorMessage(string message)
    {
        TempData.SetErrorMessage(message);
    }


    protected void HandleError(ILogger log, string message, Exception e)
    {
        log.LogError(message, e);
        AddErrorMessage(message);
    }
}