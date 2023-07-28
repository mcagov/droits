namespace Droits.Models.ViewModels.ListViews
{
    public class WreckListView : ListView<object>
    {
        public WreckListView()
        {
        }
        public WreckListView(IList<WreckView> wrecks)
        {
            Items = wrecks.Cast<object>().ToList();
        }
    }
}