#region

using Droits.Models.FormModels.SearchFormModels;

#endregion

namespace Droits.Models.ViewModels.ListViews;

public class LetterListView : ListView<object>
{
    public LetterListView()
    {
    }


    public LetterListView(IList<LetterView> letters)
    {
        Items = letters.Cast<object>().ToList();
        Letters = letters;

        SearchForm = new LetterSearchForm();
        
    }


    public IList<LetterView> Letters { get; } = new List<LetterView>();
    

}
