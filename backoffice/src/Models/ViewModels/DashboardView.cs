#region

using Droits.Models.FormModels.SearchFormModels;
using Droits.Models.ViewModels.ListViews;

#endregion

namespace Droits.Models.ViewModels;

public class DashboardView
{
    public DashboardView()
    {
        Droits = new DroitListView();
        Letters = new LetterListView();
        DashboardSearchForm = new DashboardSearchForm();
    }


    public DashboardView(DroitListView droits, LetterListView letters)
    {
        Droits = droits;
        Letters = letters;
        DashboardSearchForm = new DashboardSearchForm()
        {
            DroitsPageNumber = droits.PageNumber,
            LettersPageNumber = letters.PageNumber
        };
    }
    
    public DroitListView Droits { get; set; }
    public LetterListView Letters { get; set; }
    public DashboardSearchForm DashboardSearchForm { get; set; }
}