#region

using System.ComponentModel.DataAnnotations.Schema;
using Droits.Models.Enums;

#endregion

namespace Droits.Models.Entities;

public class WreckMaterial : BaseEntity
{
    [ForeignKey("Droit")]
    public Guid DroitId { get; set; }

    public virtual Droit? Droit { get; set; }
    public string Name { get; set; } = string.Empty;
    public Address StorageAddress { get; set; } = new();
    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
    public virtual ICollection<DroitFile> Files { get; set; } = new List<DroitFile>();

    public bool StoredAtSalvorAddress { get; set; } = false;
    public string? Description { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
    public double? SalvorValuation { get; set; } = 0;
    public bool ValueKnown { get; set; }
    public double? ReceiverValuation { get; set; } = 0;
    public bool ValueConfirmed { get; set; }

    public string? WreckMaterialOwner { get; set; } = string.Empty;
    public string? WreckMaterialOwnerContactDetails { get; set; } = string.Empty;
    
    public string? Purchaser { get; set; } = string.Empty;
    public string? PurchaserContactDetails { get; set; } = string.Empty;
    
    public WreckMaterialOutcome? Outcome { get; set; }
    public string? OutcomeRemarks { get; set; } = string.Empty;
    
    public string? PowerappsWreckMaterialId { get; set; }
    public string? PowerappsDroitId { get; set; }

}