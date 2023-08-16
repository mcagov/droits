using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;

namespace Droits.Models.FormModels;

public class WreckMaterialForm : BaseEntityForm
{
    public WreckMaterialForm()
    {
        ImageForms = new List<ImageForm>()
            { new ImageForm() { Url = "foo" }, new ImageForm() { Url = "bar" } };
    }


    public WreckMaterialForm(WreckMaterial wreckMaterial) : base(wreckMaterial)
    {
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
        StorageAddress = new AddressForm(wreckMaterial.StorageAddress);
        // if ( wreckMaterial.Images.Any() )
        // {
        //     ImageForms = 
        //         wreckMaterial.Images.Select(i => new ImageForm(i)).ToList();
        // }
        // else
        // {
        //     ImageForms.Add(new ImageForm());
        // }
        ImageForms = new List<ImageForm>()
            { new ImageForm() { Url = "foo" }, new ImageForm() { Url = "bar" } };
    }


    [Required]
    public Guid DroitId { get; set; }

    public string Name { get; set; } = string.Empty;
    public AddressForm StorageAddress { get; set; } = new();
    [Display(Name = "Is Stored At Salvor's Address?")]
    public bool StoredAtSalvor { get; set; }

    [DataType(DataType.MultilineText)]
    public string? Description { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
    public int Quantity { get; set; } = 1;

    [Range(0, float.MaxValue, ErrorMessage = "Value must be a non-negative number.")]
    public float? Value { get; set; } = 0;

    [Range(0, float.MaxValue, ErrorMessage = "Receiver valuation must be a non-negative number.")]
    [Display(Name = "Receiver Valuation")]

    public float? ReceiverValuation { get; set; } = 0;

    [Display(Name = "Value Confirmed")]
    public bool ValueConfirmed { get; set; } = false;
    
    [Display(Name = "Wreck Material Owner")]
    [DataType(DataType.MultilineText)]
    public string? WreckMaterialOwner { get; set; } = string.Empty;

    public string? Purchaser { get; set; } = string.Empty;

    [DataType(DataType.MultilineText)]
    public string? Outcome { get; set; } = string.Empty;

    [Display(Name = "Where Secured")]
    [DataType(DataType.MultilineText)]
    public string? WhereSecured { get; set; } = string.Empty;

    public List<ImageForm> ImageForms { get; set; } = new();


    public WreckMaterial ApplyChanges(WreckMaterial wreckMaterial)
    {
        
        base.ApplyChanges(wreckMaterial);
        
        wreckMaterial.DroitId = DroitId;
        wreckMaterial.Name = Name;
        wreckMaterial.Description = Description;
        wreckMaterial.Quantity = Quantity;
        wreckMaterial.Value = Value;
        wreckMaterial.ReceiverValuation = ReceiverValuation;
        wreckMaterial.ValueConfirmed = ValueConfirmed;
        // wreckMaterial.Images = Images;
        wreckMaterial.WreckMaterialOwner = WreckMaterialOwner;
        wreckMaterial.Purchaser = Purchaser;
        wreckMaterial.Outcome = Outcome;
        wreckMaterial.WhereSecured = WhereSecured;
        
        StorageAddress.ApplyChanges(wreckMaterial.StorageAddress);

        return wreckMaterial;
    }
}
