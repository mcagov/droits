using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Droits.Extensions;

public static class TempDataExtensions
{
    public static void SetSuccessMessage<T>(this ITempDataDictionary tempData, T value)
        where T : class
    {
        tempData["SuccessMessage"] = value;
    }
    public static void SetErrorMessage<T>(this ITempDataDictionary tempData, T value)
        where T : class
    {
        tempData["ErrorMessage"] = value;
    }
}
