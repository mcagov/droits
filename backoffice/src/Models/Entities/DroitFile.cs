#region

using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Droits.Models.Entities;

public class DroitFile : BaseEntity
{
    
    public string? Title { get; set; } = string.Empty;
    public string? Url { get; set; } = string.Empty;

    
    //Managed by the system
    public string? Key { get; set; } = string.Empty;
    public string? Filename { get; set; } = string.Empty;
    public string? FileContentType { get; set; } = string.Empty;
    
    public Guid? WreckMaterialId { get; set; }    
    [ForeignKey("WreckMaterialId")]
    public virtual WreckMaterial? WreckMaterial { get; set; }
    
    public Guid? NoteId { get; set; }    
    [ForeignKey("NoteId")]
    public virtual Note? Note { get; set; }

}