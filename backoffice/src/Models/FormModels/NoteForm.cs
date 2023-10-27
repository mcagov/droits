using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Droits.Models.Enums;

namespace Droits.Models.FormModels
{
    public class NoteForm : BaseEntityForm
    {
        public NoteForm()
        {
        }

        public NoteForm(Note note) : base(note)
        {
            Title = note.Title;
            Type = note.Type;
            Text = note.Text;
            DroitId = note.DroitId;
            WreckId = note.WreckId;
            SalvorId = note.SalvorId;
            LetterId = note.LetterId;
        }

        [Required(ErrorMessage = "Type is required")]
        public NoteType Type { get; set; } = NoteType.General;

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Text is required")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; } = string.Empty;

        public string ObjectReference { get; set; } = string.Empty;

        public Guid? DroitId { get; set; }

        public Guid? WreckId { get; set; }

        public Guid? SalvorId { get; set; }

        public Guid? LetterId { get; set; }

        
        public (string EntityController, Guid? EntityId) GetAssociatedEntityInfo()
        {
            if (DroitId.HasValue) return ("Droit", DroitId.Value);
            if (WreckId.HasValue) return ("Wreck", WreckId.Value);
            if (SalvorId.HasValue) return ("Salvor", SalvorId.Value);
            if (LetterId.HasValue) return ("Letter", LetterId.Value);
            return ("Note", null);
        }
        
        public Note ApplyChanges(Note note)
        {
            base.ApplyChanges(note);

            note.Type = Type;
            note.Title = Title;
            note.Text = Text;
            note.DroitId = DroitId;
            note.WreckId = WreckId;
            note.SalvorId = SalvorId;
            note.LetterId = LetterId;

            return note;
        }
    }
}