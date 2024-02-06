using Droits.Models.ViewModels.ListViews;

namespace Droits.Models.FormModels.SearchFormModels;

public class DashboardSearchForm: SearchOptions
{
    public int DroitsPageNumber { get; set; } = 1;
    public int LettersPageNumber { get; set; } = 1;
}

