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
        public string? ObjectReference { get; set; }
        public bool Editable { get; set; }
    }
}