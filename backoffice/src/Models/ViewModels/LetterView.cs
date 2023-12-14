
#region

using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Models.ViewModels.ListViews;

#endregion

namespace Droits.Models.ViewModels;

public class LetterView : BaseEntityView
{
    public LetterView(Letter letter, bool includeAssociations = false) : base(letter)
    {
        Id = letter.Id;
        Recipient = letter.Recipient;
        LetterType = letter.Type;
        Subject = letter.Subject;
        Body = letter.Body;
        SentDate = letter.DateSent;
        Status = letter.Status;
        QualityApprovedUser = letter.QualityApprovedUser?.Name;
        
        if ( includeAssociations && letter.Droit != null)
        {
            Droit = new DroitView(letter.Droit);
            Notes = new NoteListView(letter.Notes.Select(n => new NoteView(n)).OrderByDescending(n => n.Created).ToList());

        }
    }


    public Guid Id { get; }
    public DroitView? Droit { get; }
    public string Recipient { get; }
    public LetterType LetterType { get; }
    public string? QualityApprovedUser { get; }

    public string Subject { get; } 
    public string Body { get; }
    
    public LetterStatus Status { get; set; }
    private DateTime? SentDate { get; }
    public bool IsSent => SentDate.HasValue;
    public NoteListView Notes { get; } = new();
}
