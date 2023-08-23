using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;

namespace Droits.Models.ViewModels;

public class WreckMaterialView : BaseEntityView
{
    public WreckMaterialView()
    {
    }


    public WreckMaterialView(WreckMaterial wreckMaterial) : base(wreckMaterial)
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
        StorageAddress = new AddressView(wreckMaterial.StorageAddress);
        
        if ( wreckMaterial.Images.Any() )
        {
            Images = wreckMaterial.Images.Select(i => new ImageView(i)).OrderByDescending(i => i.LastModified).ToList();
        }
    }


    public Guid Id { get; }
    public Guid DroitId { get; }
    public string Name { get; } = string.Empty;
    public AddressView StorageAddress { get; } = new();

    [DataType(DataType.MultilineText)]
    public string? Description { get; } = string.Empty;
    public int Quantity { get; } = 1;
    public float? Value { get; } = 0;
    
    // Images
    public List<ImageView> Images { get; } = new();

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
    
}
