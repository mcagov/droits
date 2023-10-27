using System.ComponentModel.DataAnnotations.Schema;
using Droits.Models.Enums;

namespace Droits.Models.Entities;

public class Letter : BaseEntity
{
    public Guid DroitId { get; set; }
    
    [ForeignKey("DroitId")]
    public virtual Droit? Droit { get; set; }
    public Guid QualityApprovedUserId { get; set; }
    [ForeignKey("QualityApprovedUserId")]
    public virtual ApplicationUser? QualityApprovedUser { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Recipient { get; set; } = string.Empty;
    public LetterStatus Status { get; set; } = LetterStatus.Draft;
    public LetterType Type { get; set; } = LetterType.CustomLetter;
    public Guid SenderUserId { get; set; }
    public DateTime? DateSent { get; set; }

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

}