#region

using Droits.Models.FormModels.SearchFormModels;

#endregion

namespace Droits.Models.ViewModels.ListViews;

public class SalvorListView : ListView<object>
{
    public SalvorListView()
    {
    }


    public SalvorListView(IList<SalvorView> salvors)
    {
        Items = salvors.Cast<object>().ToList();
        SearchForm = new SalvorSearchForm();
    }


}