#region

using Droits.Models.ViewModels.ListViews;

#endregion

namespace Droits.Models.ViewModels;

public class DashboardView
{
    public DashboardView()
    {
        Droits = new DroitListView();
        Letters = new LetterListView();
    }


    public DashboardView(DroitListView droits, LetterListView letters)
    {
        Droits = droits;
        Letters = letters;
    }
    
    public DroitListView Droits { get; set; }
    public LetterListView Letters { get; set; }
}