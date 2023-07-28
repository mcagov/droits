namespace Droits.Models.ViewModels.ListViews
{
    public class LetterListView : ListView<object>
    {
        public LetterListView()
        {
        }
        public LetterListView(IList<LetterView> letters)
        {
            Items = letters.Cast<object>().ToList();
            Letters = letters;
        }


        public IList<LetterView> Letters { get; } = new List<LetterView>();
    }
}