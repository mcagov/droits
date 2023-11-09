#region

using Microsoft.AspNetCore.Mvc.ModelBinding;

#endregion

namespace Droits.Helpers.Extensions;

public static class ModelStateExtensions
{
    public static void RemoveStartingWith(this ModelStateDictionary modelState, string startsWith)
    {
        foreach ( var key in modelState.Keys.Where(k => k.StartsWith(startsWith)) )
        {
            modelState.Remove(key);
        }
    }
}