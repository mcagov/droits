using Droits.Models.FormModels.SearchFormModels;

namespace Droits.Models.ViewModels.ListViews;

public class ListView<T> : SearchOptions
{
    public List<T> Items { get; set; } = new();


    public ListView()
    {
    }


    public ListView(IList<T> items)
    {
        Items = items.ToList();
    }
    
    public SearchForm? SearchForm { get; set; }
    
}