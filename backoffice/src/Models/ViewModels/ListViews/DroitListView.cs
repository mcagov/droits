namespace Droits.Models.ViewModels.ListViews;

public class DroitListView : ListView<object>
{
    public DroitListView()
    {
    }


    public DroitListView(IList<DroitView> droits)
    {
        Items = droits.Cast<object>().ToList();
    }
}