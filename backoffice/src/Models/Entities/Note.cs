using System.ComponentModel.DataAnnotations.Schema;
using Droits.Models.Enums;

namespace Droits.Models.Entities;

public class Note : BaseEntity
{
    public NoteType Type { get; set; } = NoteType.General;

    public string Text { get; set; } = string.Empty;
    public Guid? DroitId { get; set; }
    [ForeignKey("DroitId")]
    public virtual Droit? Droit { get; set; }

    public Guid? WreckId { get; set; }
    [ForeignKey("WreckId")]
    public virtual Wreck? Wreck { get; set; }

    public Guid? SalvorId { get; set; }
    [ForeignKey("SalvorId")]
    public virtual Salvor? Salvor { get; set; }
    
    public Guid? LetterId { get; set; }
    [ForeignKey("LetterId")]
    public virtual Letter? Letter { get; set; }

}