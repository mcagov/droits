using Microsoft.AspNetCore.Mvc.Rendering;

namespace Droits.Models.FormModels;

public class FormModel
{
    public List<SelectListItem> BooleanSelectList => new()
    {
        new SelectListItem { Text = "Yes", Value = "True" },
        new SelectListItem { Text = "No", Value = "False" }
    };
}