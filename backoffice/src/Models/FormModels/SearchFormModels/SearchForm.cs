using Droits.Models.ViewModels.ListViews;

namespace Droits.Models.FormModels.SearchFormModels;

public class SearchForm : SearchOptions
{
    public string? SubmitAction { get; set; } = "Search";
}