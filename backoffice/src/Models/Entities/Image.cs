using System.ComponentModel.DataAnnotations.Schema;

namespace Droits.Models.Entities;

public class Image : BaseEntity
{
    public string Url { get; set; } = string.Empty;
    
    public Guid? WreckMaterialId { get; set; }    
    [ForeignKey("WreckMaterialId")]
    public virtual WreckMaterial? WreckMaterial { get; set; }

}