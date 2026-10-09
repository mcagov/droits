namespace Droits.Models.FormModels.SearchFormModels;

public class DashboardSearchForm : DroitSearchForm
{
    public int DroitsPageNumber { get; set; } = 1;
    public int LettersPageNumber { get; set; } = 1;
    public bool ApplyFilters { get; set; }
}
