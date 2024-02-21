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
    
    // Below may move somewhere else
    protected void AddTriageSuccessMessage(string message)
    {
        TempData.SetTriageSuccessMessage(message);
    }


    protected void AddTriageErrorMessage(string message)
    {
        TempData.SetTriageErrorMessage(message);
    }



    protected void HandleError(ILogger log, string message, Exception e)
    {
        log.LogError(message, e);
        AddErrorMessage(message);
    }
}