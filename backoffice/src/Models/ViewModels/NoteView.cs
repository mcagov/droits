using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Droits.Models.Enums;

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

        // Relationships
        public Guid? DroitId { get; }
        public Guid? WreckId { get; }
        public Guid? SalvorId { get; }
        public Guid? LetterId { get; }
    }
}