using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Droits.Models.Enums;

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
        
        if ( includeAssociations && letter.Droit != null)
        {
            Droit = new DroitView(letter.Droit);
        }
    }


    public Guid Id { get; }
    public DroitView? Droit { get; }
    public string Recipient { get; }
    public LetterType LetterType { get; }

    public string Subject { get; } 
    public string Body { get; }
    private DateTime? SentDate { get; }
    public bool IsSent => SentDate.HasValue;

}
