#region

using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Droits.Models.Enums;

#endregion

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
        SalvorValuation = wreckMaterial.SalvorValuation;
        ReceiverValuation = wreckMaterial.ReceiverValuation;
        ValueConfirmed = wreckMaterial.ValueConfirmed;
        // Images = wreckMaterial.Images;
        WreckMaterialOwner = wreckMaterial.WreckMaterialOwner;
        WreckMaterialOwnerContactDetails = wreckMaterial.WreckMaterialOwnerContactDetails;
        Purchaser = wreckMaterial.Purchaser;
        PurchaserContactDetails = wreckMaterial.PurchaserContactDetails;
        Outcome = wreckMaterial.Outcome;
        OutcomeRemarks = wreckMaterial.OutcomeRemarks;
        StorageAddress = new AddressView(wreckMaterial.StorageAddress);
        
        if ( wreckMaterial.Images.Any() )
        {
            Images = wreckMaterial.Images.Select(i => new ImageView(i)).OrderByDescending(i => i.Created).ToList();
        }
        if ( wreckMaterial.Files.Any() )
        {
            Files = wreckMaterial.Files.Select(i => new DroitFileView(i)).OrderByDescending(i => i.Created).ToList();
        }
    }


    public Guid Id { get; }
    public Guid DroitId { get; }
    public string Name { get; } = string.Empty;
    public AddressView StorageAddress { get; } = new();

    [DataType(DataType.MultilineText)]
    public string? Description { get; set; } = string.Empty;
    public int Quantity { get; } = 1;
    [Display(Name = "Salvor Valuation")]
    public double? SalvorValuation { get; } = 0;
    
    // Images
    public List<ImageView> Images { get; } = new();
    public List<DroitFileView> Files { get; } = new();


    [Display(Name = "Receiver Valuation")]
    public double? ReceiverValuation { get; }

    [Display(Name = "Value Confirmed")]
    public bool ValueConfirmed { get; }
    // public List<string> Images { get; } = new List<string>();

    [Display(Name = "Owner")]
    public string? WreckMaterialOwner { get; set; } = string.Empty;
    
    [Display(Name = "Owner Contact Details")]
    [DataType(DataType.MultilineText)]
    public string? WreckMaterialOwnerContactDetails { get; set; } = string.Empty;
    
    
    [Display(Name = "Purchaser")]
    public string? Purchaser { get; set; } = string.Empty;
    
    [Display(Name = "Purchaser Contact Details")]
    [DataType(DataType.MultilineText)]
    public string? PurchaserContactDetails { get; set; } = string.Empty;
    
    public WreckMaterialOutcome? Outcome { get; set; }
    
    [Display(Name = "Outcome Remarks")]
    [DataType(DataType.MultilineText)]
    public string? OutcomeRemarks { get; set; } = string.Empty;
    
}
