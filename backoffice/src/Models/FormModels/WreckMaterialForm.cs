#region

using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Droits.Models.Enums;

#endregion

namespace Droits.Models.FormModels;

public class WreckMaterialForm : BaseEntityForm
{
    public WreckMaterialForm()
    {
        
    }


    public WreckMaterialForm(WreckMaterial wreckMaterial) : base(wreckMaterial)
    {
        DroitId = wreckMaterial.DroitId;
        Name = wreckMaterial.Name;
        Description = wreckMaterial.Description;
        Quantity = wreckMaterial.Quantity;
        SalvorValuation = wreckMaterial.SalvorValuation;
        ReceiverValuation = wreckMaterial.ReceiverValuation;
        ValueConfirmed = wreckMaterial.ValueConfirmed;
        WreckMaterialOwner = wreckMaterial.WreckMaterialOwner;
        WreckMaterialOwnerContactDetails = wreckMaterial.WreckMaterialOwnerContactDetails;
        Purchaser = wreckMaterial.Purchaser;
        PurchaserContactDetails = wreckMaterial.PurchaserContactDetails;
        Outcome = wreckMaterial.Outcome;
        OutcomeRemarks = wreckMaterial.OutcomeRemarks;
        StoredAtSalvorAddress = wreckMaterial.StoredAtSalvorAddress;
        StorageAddress = new AddressForm(wreckMaterial.StorageAddress);
        if ( wreckMaterial.Images.Any() )
        {
            ImageForms = 
                wreckMaterial.Images.Select(i => new ImageForm(i)).ToList();
        }
        if ( wreckMaterial.Files.Any() )
        {
            DroitFileForms = 
                wreckMaterial.Files.Select(f => new DroitFileForm(f)).ToList();
        }
    }


    [Required]
    public Guid DroitId { get; set; }

    public string Name { get; set; } = string.Empty;
    public AddressForm? StorageAddress { get; set; } = new();
    [Display(Name = "Is Stored At Salvor's Address?")]
    public bool StoredAtSalvorAddress { get; set; }

    [DataType(DataType.MultilineText)]
    public string? Description { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
    public int Quantity { get; set; } = 1;

    [Range(0, double.MaxValue, ErrorMessage = "Value must be a non-negative number.")]
    [Display(Name = "Salvor Valuation")]
    public double? SalvorValuation { get; set; } = 0;

    [Range(0, double.MaxValue, ErrorMessage = "Receiver valuation must be a non-negative number.")]
    [Display(Name = "Receiver Valuation")]

    public double? ReceiverValuation { get; set; } = 0;

    [Display(Name = "Value Confirmed")]
    public bool ValueConfirmed { get; set; } = false;
    
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

    [Display(Name = "Where Secured")]
    [DataType(DataType.MultilineText)]
    public string? WhereSecured { get; set; } = string.Empty;

    public List<ImageForm> ImageForms { get; set; } = new();
    public List<DroitFileForm> DroitFileForms { get; set; } = new();

    
    public WreckMaterial ApplyChanges(WreckMaterial wreckMaterial)
    {
        
        base.ApplyChanges(wreckMaterial);
        
        wreckMaterial.DroitId = DroitId;
        wreckMaterial.Name = Name;
        wreckMaterial.Description = Description;
        wreckMaterial.Quantity = Quantity;
        wreckMaterial.SalvorValuation = SalvorValuation;
        wreckMaterial.ReceiverValuation = ReceiverValuation;
        wreckMaterial.ValueConfirmed = ValueConfirmed;
        
        wreckMaterial.WreckMaterialOwner = WreckMaterialOwner;
        wreckMaterial.WreckMaterialOwnerContactDetails = WreckMaterialOwnerContactDetails;

        wreckMaterial.Purchaser = Purchaser;
        wreckMaterial.PurchaserContactDetails = PurchaserContactDetails;
        
        wreckMaterial.Outcome = Outcome;
        wreckMaterial.OutcomeRemarks = OutcomeRemarks;
        
        wreckMaterial.StoredAtSalvorAddress = StoredAtSalvorAddress;

        StorageAddress ??= new AddressForm();
        StorageAddress.ApplyChanges(wreckMaterial.StorageAddress);
        

        return wreckMaterial;
    }
}
