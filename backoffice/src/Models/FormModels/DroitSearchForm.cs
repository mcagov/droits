using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Droits.Models.FormModels;

public class DroitSearchForm
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
    public float? LatitudeFrom { get; set; }

    [DisplayName("Latitude To")]
    public float? LatitudeTo { get; set; }

    [DisplayName("Longitude From")]
    public float? LongitudeFrom { get; set; }

    [DisplayName("Longitude To")]
    public float? LongitudeTo { get; set; }

    [DisplayName("In UK Waters")]
    public bool? InUkWaters { get; set; }

    [DisplayName("Location Radius")]
    public int? LocationRadius { get; set; }

    [DisplayName("Depth From (metres)")]
    public float? DepthFrom { get; set; }
    
    [DisplayName("Depth To (metres)")]
    public float? DepthTo { get; set; }

    [DisplayName("Recovered From")]
    public RecoveredFrom? RecoveredFrom { get; set; }

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
    public float? ValueFrom { get; set; } 
    
    [DisplayName("Value To")]
    public float? ValueTo { get; set; } 
    
    [DisplayName("Receiver Valuation From")]
    public float? ReceiverValuationFrom { get; set; } 
    
    [DisplayName("Receiver Valuation To")]
    public float? ReceiverValuationTo { get; set; } 

    // Salvage

    [DisplayName("Salvage Award Claimed")]
    public bool? SalvageAwardClaimed { get; set; }

    [DisplayName("Services Description")]
    public string? ServicesDescription { get; set; }

    [DisplayName("Services Duration")]
    public string? ServicesDuration { get; set; } //Units? Unknown.

    [DisplayName("Services Estimated Cost")]

    public float? ServicesEstimatedCost { get; set; }

    [DisplayName("Services Estimate Cost To")]
    public float? ServicesEstimatedCostTo { get; set; }
    
    [DisplayName("Services Estimate Cost From")]
    public float? ServicesEstimatedCostFrom { get; set; }
    
    [DisplayName("Salvage Claim Awarded From")]
    public float? SalvageClaimAwardedFrom { get; set; }
    
    [DisplayName("Salvage Claim Awarded To")]
    public float? SalvageClaimAwardedTo { get; set; }

    [DisplayName("MMO Licence Required")]
    public bool? MMOLicenceRequired { get; set; }

    [DisplayName("MMO Licence Provided")]
    public bool? MMOLicenceProvided { get; set; }

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
}
    