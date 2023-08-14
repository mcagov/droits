namespace Droits.Models.ViewModels.ListViews
{
    public class NoteListView : ListView<object>
    {
        public NoteListView()
        {
        }

        public NoteListView(IList<NoteView> notes)
        {
            Items = notes.Cast<object>().ToList();
            Notes = notes;

        }
        
        public IList<NoteView> Notes { get; } = new List<NoteView>();

    }
}