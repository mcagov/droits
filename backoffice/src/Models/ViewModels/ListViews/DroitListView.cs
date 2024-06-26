
#region
using Droits.Models.FormModels.SearchFormModels;

#endregion

namespace Droits.Models.ViewModels.ListViews;

public class DroitListView : ListView<object>
{
    public DroitListView()
    {
    }


    public DroitListView(IList<DroitView> droits)
    {
        Items = droits.Cast<object>().ToList();
        SearchForm = new DroitSearchForm();
        AnyVerifiedWrecks = droits.Any(d => d.Wreck?.Name != null);
    }


    public bool AnyVerifiedWrecks { get; } = false;


}