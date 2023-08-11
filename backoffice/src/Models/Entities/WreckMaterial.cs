using System.ComponentModel.DataAnnotations.Schema;

namespace Droits.Models.Entities;

public class WreckMaterial : BaseEntity
{
    [ForeignKey("Droit")]
    public Guid DroitId { get; set; }

    public virtual Droit? Droit { get; set; }
    public string Name { get; set; } = string.Empty;
    public Address StorageAddress { get; set; } = new();
    public string? Description { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
    public float? Value { get; set; } = 0;
    public float? ReceiverValuation { get; set; } = 0;

    public bool ValueConfirmed { get; set; } = false;
    // public List<string> Images { get; set; } = new List<string>();


    // public string StorageAddress {get;set;} = string.Empty;

    // Legacy fields ..

    public string? WreckMaterialOwner { get; set; } = string.Empty;
    public string? Purchaser { get; set; } = string.Empty;
    public string? Outcome { get; set; } = string.Empty;
    public string? WhereSecured { get; set; } = string.Empty;
}