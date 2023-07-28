using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;

namespace Droits.Models.ViewModels;

public class WreckMaterialView
{
    public WreckMaterialView()
    {
    }


    public WreckMaterialView(WreckMaterial wreckMaterial)
    {
        Id = wreckMaterial.Id;
        DroitId = wreckMaterial.DroitId;
        Name = wreckMaterial.Name;
        Description = wreckMaterial.Description;
        Quantity = wreckMaterial.Quantity;
        Value = wreckMaterial.Value;
        ReceiverValuation = wreckMaterial.ReceiverValuation;
        ValueConfirmed = wreckMaterial.ValueConfirmed;
        // Images = wreckMaterial.Images;
        WreckMaterialOwner = wreckMaterial.WreckMaterialOwner;
        Purchaser = wreckMaterial.Purchaser;
        Outcome = wreckMaterial.Outcome;
        WhereSecured = wreckMaterial.WhereSecured;
        Created = wreckMaterial.Created;
        LastModified = wreckMaterial.LastModified;
        StorageAddress = new AddressView(wreckMaterial.StorageAddress);
    }


    public Guid Id { get; }
    public Guid DroitId { get; }
    public string Name { get; } = string.Empty;
    public AddressView StorageAddress { get; } = new();

    [DataType(DataType.MultilineText)]
    public string? Description { get; } = string.Empty;
    public int Quantity { get; } = 1;
    public float? Value { get; } = 0;


    [Display(Name = "Receiver Valuation")]
    public float? ReceiverValuation { get; }

    [Display(Name = "Value Confirmed")]
    public bool ValueConfirmed { get; }
    // public List<string> Images { get; } = new List<string>();

    [Display(Name = "Wreck Material Owner")]
    [DataType(DataType.MultilineText)]
    public string? WreckMaterialOwner { get; } = string.Empty;

    public string? Purchaser { get; } = string.Empty;

    [DataType(DataType.MultilineText)]
    public string? Outcome { get; } = string.Empty;

    [Display(Name = "Where Secured")]
    [DataType(DataType.MultilineText)]
    public string? WhereSecured { get; } = string.Empty;

    public DateTime Created { get; }

    [Display(Name = "Last Modified")]
    public DateTime LastModified { get; }
}
