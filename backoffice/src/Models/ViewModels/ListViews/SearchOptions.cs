namespace Droits.Models.ViewModels;

public class SearchOptions
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public bool IncludeAssociations { get; set; } = false;
    public int TotalCount { get; set; }
}

