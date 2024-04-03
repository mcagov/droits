#region

using Droits.Models.FormModels.SearchFormModels;
using Droits.Models.ViewModels.ListViews;

#endregion

namespace Droits.Models.ViewModels;

public class MetricsDashboardView
{
    
    // Closed droits figures by month & year 
    // Open droits figures by month & year 
    // Triage totals by type / month and year
    // Closure totals by type / month & year
    public MetricsDashboardView()
    {
        Droits = new DroitListView();
        Letters = new LetterListView();
        DashboardSearchForm = new DashboardSearchForm();
    }


    public MetricsDashboardView(DroitListView droits, LetterListView letters)
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