using System.ComponentModel.DataAnnotations.Schema;

namespace Droits.Models.Entities;

public class Image : BaseEntity
{
    
    public string Title { get; set; } = string.Empty;
    
    
    //Managed by the system
    public string Key { get; set; } = string.Empty;
    public string Filename { get; set; } = string.Empty;
    public string FileContentType { get; set; } = string.Empty;
    
    public Guid? WreckMaterialId { get; set; }    
    [ForeignKey("WreckMaterialId")]
    public virtual WreckMaterial? WreckMaterial { get; set; }

}