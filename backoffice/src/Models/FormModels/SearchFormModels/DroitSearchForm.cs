#region

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

#endregion

namespace Droits.Models.FormModels.SearchFormModels;

public class DroitSearchForm : SearchForm
{
    public DroitSearchForm()
    {

    }


    // Base fields...
    // public WreckForm WreckForm { get; set; } = new();


    // public List<WreckMaterialForm> WreckMaterialForms { get; set; } = new();
    // public SalvorForm SalvorForm { get; set; } = new();

    public string? Reference { get; set; }

    [DisplayName("Created From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? CreatedFrom { get; set; }

    [DisplayName("Created To")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? CreatedTo { get; set; }


    [DisplayName("Last Modified From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? LastModifiedFrom { get; set; }

    [DisplayName("Last Modified To")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? LastModifiedTo { get; set; }


    public List<DroitStatus> StatusList { get; set; } = new();
    public List<int> SelectedStatusList => StatusList.Select(s => ( int )s).ToList();

    [DisplayName("Assigned To")]
    public Guid? AssignedToUserId { get; set; }

    public List<SelectListItem> AssignedToUsers { get; set; } = new();

    [DisplayName("Reported Date From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? ReportedDateFrom { get; set; }

    [DisplayName("Reported Date To")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? ReportedDateTo { get; set; }

    [DisplayName("Date Found From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? DateFoundFrom { get; set; }

    [DisplayName("Date Found To")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? DateFoundTo { get; set; }



    // Wreck


    [DisplayName("Wreck Name")]
    public string? WreckName { get; set; }

    [DisplayName("Is Isolated Find")]
    public bool? IsIsolatedFind { get; set; }

    // public Guid? WreckId { get; set; }
    //
    [DisplayName("Is Hazardous Find")]
    public bool? IsHazardousFind { get; set; }

    [DisplayName("Is Dredge")]
    public bool? IsDredge { get; set; }

    // Salvor

    [DisplayName("Salvor Name")]
    public string? SalvorName { get; set; }
    // public Guid? SalvorId { get; set; }


    // Location

    [DisplayName("Latitude From")]
    public double? LatitudeFrom { get; set; }

    [DisplayName("Latitude To")]
    public double? LatitudeTo { get; set; }

    [DisplayName("Longitude From")]
    public double? LongitudeFrom { get; set; }

    [DisplayName("Longitude To")]
    public double? LongitudeTo { get; set; }

    [DisplayName("In UK Waters")]
    public bool? InUkWaters { get; set; }

    [DisplayName("Location Radius")]
    public int? LocationRadius { get; set; }

    [DisplayName("Depth From (metres)")]
    public double? DepthFrom { get; set; }
    
    [DisplayName("Depth To (metres)")]
    public double? DepthTo { get; set; }

    [DisplayName("Recovered From")]
    public List<RecoveredFrom> RecoveredFromList { get; set; } = new();
    public List<int> SelectedRecoveredFromList => RecoveredFromList.Select(r => ( int )r).ToList();


    [DisplayName("Location Description")]
    public string? LocationDescription { get; set; } = string.Empty;

    // Wreck Material

    [DisplayName("Wreck Material")]
    public string? WreckMaterial { get; set; } = string.Empty;

    [DisplayName("Wreck Material Owner")]
    public string? WreckMaterialOwner { get; set; } = string.Empty;
    
    [DisplayName("Stored at Salvor Address")]
    public bool? StoredAtSalvorAddress { get; set; }
    
    [DisplayName("Value Confirmed")]
    public bool? ValueConfirmed { get; set; }
    
    [DisplayName("Quantity From")]
    public int? QuantityFrom { get; set; } 
    
    [DisplayName("Quantity To")]
    public int? QuantityTo { get; set; } 
    
    [DisplayName("Value From")]
    public double? ValueFrom { get; set; } 
    
    [DisplayName("Value To")]
    public double? ValueTo { get; set; } 
    
    [DisplayName("Receiver Valuation From")]
    public double? ReceiverValuationFrom { get; set; } 
    
    [DisplayName("Receiver Valuation To")]
    public double? ReceiverValuationTo { get; set; } 

    // Salvage

    [DisplayName("Salvage Award Claimed")]
    public bool? SalvageAwardClaimed { get; set; }

    [DisplayName("Services Description")]
    public string? ServicesDescription { get; set; }

    [DisplayName("Services Duration")]
    public string? ServicesDuration { get; set; } //Units? Unknown.

    [DisplayName("Services Estimated Cost")]

    public double? ServicesEstimatedCost { get; set; }

    [DisplayName("Services Estimate Cost To")]
    public double? ServicesEstimatedCostTo { get; set; }
    
    [DisplayName("Services Estimate Cost From")]
    public double? ServicesEstimatedCostFrom { get; set; }
    
    [DisplayName("Salvage Claim Awarded From")]
    public double? SalvageClaimAwardedFrom { get; set; }
    
    [DisplayName("Salvage Claim Awarded To")]
    public double? SalvageClaimAwardedTo { get; set; }

    [DisplayName("MMO Licence Required")]
    public bool? MmoLicenceRequired { get; set; }

    [DisplayName("MMO Licence Provided")]
    public bool? MmoLicenceProvided { get; set; }

    // Legacy fields
    public string? District { get; set; }

    [DisplayName("Legacy File Reference")]
    public string? LegacyFileReference { get; set; } //Physical file location/ref

    [DisplayName("Goods Discharged By")]
    public string? GoodsDischargedBy { get; set; } //Initals of RoW member.

    [DisplayName("Date Delivered")]
    public string? DateDelivered { get; set; } //Unsure of date format.

    public string? Agent { get; set; }

    [DisplayName("Recovered From")]
    public string? RecoveredFromLegacy { get; set; }

    [DisplayName("Imported From Legacy")]
    public bool? ImportedFromLegacy { get; set; }

    public bool IgnoreWreckMaterialSearch => new List<bool>
    {
        WreckMaterial.IsNullOrEmpty(),
        QuantityFrom == null,
        QuantityTo == null,
        ValueFrom == null,
        ValueTo == null,
        ReceiverValuationFrom == null,
        ReceiverValuationTo == null,
        WreckMaterialOwner.IsNullOrEmpty(),
        ValueConfirmed == null
    }.All(result => result); 
}
    