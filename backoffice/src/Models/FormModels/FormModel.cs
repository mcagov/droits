using Microsoft.AspNetCore.Mvc.Rendering;

namespace Droits.Models.FormModels;

public class FormModel
{
    public List<SelectListItem> BooleanSelectList => new()
    {
        new() { Text = "Yes", Value = "True" },
        new() { Text = "No", Value = "False" }
    };
}