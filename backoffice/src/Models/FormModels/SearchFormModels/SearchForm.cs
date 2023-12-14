#region

using Droits.Models.ViewModels.ListViews;

#endregion

namespace Droits.Models.FormModels.SearchFormModels;

public class SearchForm : SearchOptions
{
    public string? SubmitAction { get; set; } = "Search";
}