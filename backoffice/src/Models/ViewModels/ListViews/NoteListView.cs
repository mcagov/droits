namespace Droits.Models.ViewModels.ListViews
{
    public class NoteListView : ListView<object>
    {
        public NoteListView()
        {
        }

        public NoteListView(IList<NoteView> notes, bool editable = true)
        {
            Items = notes.Cast<object>().ToList();
            Notes = notes;
            Editable = editable;
        }
        
        public IList<NoteView> Notes { get; } = new List<NoteView>();
        public bool Editable { get; set; }
    }
}