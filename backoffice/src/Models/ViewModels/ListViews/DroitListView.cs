using Droits.Models.FormModels;
using Droits.Models.FormModels.SearchFormModels;

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
    }
    
}