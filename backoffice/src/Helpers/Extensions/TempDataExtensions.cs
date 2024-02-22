#region

using Microsoft.AspNetCore.Mvc.ViewFeatures;

#endregion

namespace Droits.Helpers.Extensions;

public static class TempDataExtensions
{
    public static void SetSuccessMessage<T>(this ITempDataDictionary tempData, T message)
    {
        tempData["SuccessMessage"] = message;
    }


    public static void SetErrorMessage<T>(this ITempDataDictionary tempData, T message)
    {
        tempData["ErrorMessage"] = message;
    }
    

    public static void Add<T>(this ITempDataDictionary tempData, string key, string value)
    {
        tempData[key] = value;
    }
}