
#region

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Droits.Models.Enums;

#endregion

namespace Droits.Models.ViewModels
{
    public class NoteView : BaseEntityView
    {
        public NoteView()
        {
        }

        public NoteView(Note note, bool includeAssociations = false) : base(note)
        {
            Id = note.Id;
            Type = note.Type;
            Title = note.Title;
            Text = note.Text;
            DroitId = note.DroitId;
            WreckId = note.WreckId;
            SalvorId = note.SalvorId;
            LetterId = note.LetterId;

            if ( note.Files.Any() )
            {
                Files = note.Files.Select(i => new DroitFileView(i)).OrderByDescending(i => i.Created).ToList();
            }
            
            if (includeAssociations)
            {
                
            }
        }

        public Guid Id { get; }
        [DisplayName("Type")]
        public NoteType Type { get; } = NoteType.General;

        [DisplayName("Title")]
        public string Title { get; } = string.Empty;
        
        [DisplayName("Note")]
        [DataType(DataType.MultilineText)]
        public string Text { get; } = string.Empty;
        public string ObjectReference { get; set; } = string.Empty;

        // Relationships
        public Guid? DroitId { get; }
        public Guid? WreckId { get; }
        public Guid? SalvorId { get; }
        public Guid? LetterId { get; }
        public List<DroitFileView> Files { get; } = new();
        public (string EntityController, Guid? EntityId) GetAssociatedEntityInfo()
        {
            if (DroitId.HasValue) return ("Droit", DroitId.Value);
            if (WreckId.HasValue) return ("Wreck", WreckId.Value);
            if (SalvorId.HasValue) return ("Salvor", SalvorId.Value);
            if (LetterId.HasValue) return ("Letter", LetterId.Value);
            return ("Note", null);
        }
    }
}