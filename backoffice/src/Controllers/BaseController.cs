using Microsoft.AspNetCore.Mvc;
using Droits.Extensions;

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

    protected void HandleError(ILogger log, string message, Exception e){
        log.LogError(message,e);
        AddErrorMessage(message);
    }
}
