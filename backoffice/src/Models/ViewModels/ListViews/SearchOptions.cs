namespace Droits.Models.ViewModels.ListViews;

public class SearchOptions
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public bool IncludeAssociations { get; set; } = false;
    public int TotalCount { get; set; }
    public bool FilterByAssignedUser { get; set; }
    public bool SearchOpen { get; set; } = false;
}

