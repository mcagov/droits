using Droits.Models.ViewModels.ListViews;

namespace Droits.Models.ViewModels;

public class DashboardView
{
    public DashboardView() {
        
    }


    public DashboardView(DroitListView droits, LetterListView letters)
    {
        Droits = droits;
        Letters = letters;
    }
    
    public DroitListView Droits { get; set; }
    public LetterListView Letters { get; set; }
}